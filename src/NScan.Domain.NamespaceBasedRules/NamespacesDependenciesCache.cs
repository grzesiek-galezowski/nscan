﻿using System.Collections.Generic;
using System.Linq;
using LanguageExt;
using NScan.Lib;

namespace NScan.NamespaceBasedRules;

public class NamespacesDependenciesCache : INamespacesDependenciesCache
{
  private readonly 
    SortedDictionary<NamespaceName, List<NamespaceName>> _dependenciesByNamespace = new();

  //bug adjacency list accepts duplicates but should not!
  public void AddMapping(NamespaceName namespaceName, NamespaceName usingName)
  {
    InitializeNeighborsOf(namespaceName);
    AddNeighborOf(namespaceName, usingName);
  }

  public Arr<NamespaceDependencyPath> RetrieveCycles()
  {
    var cycles = new List<NamespaceDependencyPath>();
    foreach (var @namespace in _dependenciesByNamespace.Keys)
    {
      SearchForNextElementInCycle(
        cycles, 
        @namespace, 
        NamespaceDependencyPath.Empty());
    }
    return cycles.ToArr();
  }

  public Arr<NamespaceDependencyPath> RetrievePathsBetween(Pattern fromPattern, Pattern toPattern)
  {
    var paths = new List<NamespaceDependencyPath>();
    foreach (var @namespace in NamespacesMatching(fromPattern))
    {
      SearchForNextElementInPath(
        paths, 
        NamespaceDependencyPath.Empty(), 
        toPattern, 
        @namespace);
    }
    return paths.ToArr();
  }

  private IEnumerable<NamespaceName> NamespacesMatching(Pattern fromPattern)
  {
    return _dependenciesByNamespace.Keys.Where(name => name.Matches(fromPattern));
  }

  private void AddNeighborOf(NamespaceName namespaceName, NamespaceName usingName)
  {
    if (!_dependenciesByNamespace[namespaceName].Contains(usingName))
    {
      _dependenciesByNamespace[namespaceName].Add(usingName);
    }
  }

  private void InitializeNeighborsOf(NamespaceName namespaceName)
  {
    if (!_dependenciesByNamespace.ContainsKey(namespaceName))
    {
      _dependenciesByNamespace[namespaceName] = new List<NamespaceName>();
    }
  }

  private void SearchForNextElementInPath(
    ICollection<NamespaceDependencyPath> paths,
    NamespaceDependencyPath currentPath,
    Pattern toPattern,
    NamespaceName namespaceName)
  {
    if (currentPath.HasElements() && namespaceName.Matches(toPattern))
    {
      paths.Add(currentPath.Plus(namespaceName));
      return;
    }

    if (NoDependenciesFrom(namespaceName))
    {
      return;
    }

    foreach (var neighbor in _dependenciesByNamespace[namespaceName])
    {
      SearchForNextElementInPath(
        paths, 
        currentPath.Plus(namespaceName), 
        toPattern, 
        neighbor);
    }
  }

  private void SearchForNextElementInCycle(
    ICollection<NamespaceDependencyPath> alreadyDetectedCycles, 
    NamespaceName current, 
    NamespaceDependencyPath namespaceDependencyPath)
  {
    if (NoDependenciesFrom(current))
    {
      return;
    }

    var pathIncludingCurrentElement = namespaceDependencyPath.Plus(current);

    if (pathIncludingCurrentElement.IsPathToItself())
    {
      return;
    }

    //full cycle detected
    if (pathIncludingCurrentElement.FormsACycleFromFirstElement())
    {
      //full cycle is a cycle that starts with current namespace, e.g. A->B->A

      //A->B->A and B->A->B are the same cycle, no need to report twice
      if (pathIncludingCurrentElement.IsEquivalentToAnyOf(alreadyDetectedCycles))
      {
        alreadyDetectedCycles.Add(pathIncludingCurrentElement);
      }

      return;
    }

    //overgrown cycle detection
    if(pathIncludingCurrentElement.ContainsACycleButNotFromFirstElement())
    {
      //overgrown cycles are paths that contains other cycles, e.g. A->B->C->B   
      return;
    }

    foreach (var dependency in DependenciesOf(current))
    {
      SearchForNextElementInCycle(alreadyDetectedCycles, dependency, pathIncludingCurrentElement);
    }
  }

  private IReadOnlyList<NamespaceName> DependenciesOf(NamespaceName namespaceName)
  {
    return _dependenciesByNamespace[namespaceName];
  }

  private bool NoDependenciesFrom(NamespaceName namespaceName)
  {
    return !_dependenciesByNamespace.ContainsKey(namespaceName);
  }
}
