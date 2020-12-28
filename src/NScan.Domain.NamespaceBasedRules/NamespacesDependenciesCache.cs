using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using NScan.Lib;

namespace NScan.NamespaceBasedRules
{
  public class NamespacesDependenciesCache : INamespacesDependenciesCache
  {
    private readonly 
      Dictionary<NamespaceName, List<NamespaceName>> _dependenciesByNamespace = new();

    //bug adjacency list accepts duplicates but should not!
    public void AddMapping(NamespaceName namespaceName, NamespaceName usingName)
    {
      InitializeNeighborsOf(namespaceName);
      AddNeighborOf(namespaceName, usingName);
    }

    public List<NamespaceDependencyPath> RetrieveCycles()
    {
      //TODO return cycles as well
      var cycles = new List<NamespaceDependencyPath>();
      foreach (var @namespace in _dependenciesByNamespace.Keys)
      {
        SearchForNextElementInCycle(
          cycles, 
          @namespace, 
          NamespaceDependencyPath.Empty());
      }
      return cycles; //bug
    }

    public List<NamespaceDependencyPath> RetrievePathsBetween(Pattern fromPattern, Pattern toPattern)
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
      return paths;
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
      List<NamespaceDependencyPath> alreadyDetectedCycles, 
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

    private List<NamespaceName> DependenciesOf(NamespaceName namespaceName)
    {
      return _dependenciesByNamespace[namespaceName];
    }

    private bool NoDependenciesFrom(NamespaceName namespaceName)
    {
      return !_dependenciesByNamespace.ContainsKey(namespaceName);
    }
  }

  public static class KeyValuePairDeconstruction
  {
    public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
    {
      key = tuple.Key;
      value = tuple.Value;
    }
  }
}
