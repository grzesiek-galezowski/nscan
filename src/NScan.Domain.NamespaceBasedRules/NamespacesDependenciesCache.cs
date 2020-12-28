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


    public IReadOnlyList<IReadOnlyList<NamespaceName>> RetrieveCycles()
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
      return cycles.Select(c => c.AsList()).ToList(); //bug
    }

    public IReadOnlyList<IReadOnlyList<NamespaceName>> 
      RetrievePathsBetween(Pattern fromPattern, Pattern toPattern)
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
      return paths.Select(p => p.AsList()).ToList();
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
      List<NamespaceDependencyPath> cycles, 
      NamespaceName current, 
      NamespaceDependencyPath namespaceDependencyPath)
    {
      if (NoDependenciesFrom(current))
      {
        return;
      }

      if (namespaceDependencyPath.ConsistsSolelyOf(current))
      {
        return;
      }

      //full cycle detected
      if (namespaceDependencyPath.BeginsWith(current))
      {
        //full cycle is a cycle that starts with current namespace, e.g. A->B->A
        if (CycleIsNotReportedAlreadyAsStartingFromDifferentElement(namespaceDependencyPath, cycles))
        {
          cycles.Add(namespaceDependencyPath.Plus(current));
        }

        return;
      }

      //overgrown cycle detection
      if(namespaceDependencyPath.ContainsButDoesNotBeginWith(current))
      {
        //overgrown cycles are paths that contains other cycles, e.g. A->B->C->B   
        return;
      }

      foreach (var dependency in DependenciesOf(current))
      {
        SearchForNextElementInCycle(cycles, dependency, namespaceDependencyPath.Plus(current));
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

    private static bool CycleIsNotReportedAlreadyAsStartingFromDifferentElement(
      NamespaceDependencyPath namespaceDependencyPath,
      IEnumerable<NamespaceDependencyPath> cycles)
    {
      //A->B->A and B->A->B are the same cycle, no need to report twice
      return !cycles.Any(c => c.IsEquivalentTo(namespaceDependencyPath));
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
