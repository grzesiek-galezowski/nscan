using System.Collections.Generic;
using System.Linq;
using NScan.Lib;

namespace NScan.NamespaceBasedRules
{
  public class NamespacesDependenciesCache : INamespacesDependenciesCache
  {
    private readonly Dictionary<NamespaceName, List<NamespaceName>> _adjacencyList = new();

    //bug adjacency list accepts duplicates but should not!
    public void AddMapping(NamespaceName namespaceName, NamespaceName usingName)
    {
      InitializeNeighborsOf(namespaceName);
      AddNeighborOf(namespaceName, usingName);
    }

    private void AddNeighborOf(NamespaceName namespaceName, NamespaceName usingName)
    {
      if (!_adjacencyList[namespaceName].Contains(usingName))
      {
        _adjacencyList[namespaceName].Add(usingName);
      }
    }

    //bug is NamespaceName OK? What about static usings?
    private void InitializeNeighborsOf(NamespaceName namespaceName)
    {
      if (!_adjacencyList.ContainsKey(namespaceName))
      {
        _adjacencyList[namespaceName] = new List<NamespaceName>();
      }
    }

    public IReadOnlyList<IReadOnlyList<NamespaceName>> RetrieveCycles()
    {
      //TODO return cycles as well
      var cycles = new List<List<NamespaceName>>();
      foreach (var @namespace in _adjacencyList.Keys)
      {
        SearchForNextElementInCycle(
          cycles, 
          @namespace, 
          CurrentPath.Empty());
      }
      return cycles;
    }

    public IReadOnlyList<IReadOnlyList<NamespaceName>> 
      RetrievePathsBetween(Pattern fromPattern, Pattern toPattern)
    {
      var paths = new List<List<NamespaceName>>();
      foreach (var @namespace in _adjacencyList.Keys.Where(k => k.Matches(fromPattern)))
      {
        SearchForNextElementInPath(
          paths, 
          new List<NamespaceName>(), 
          toPattern, @namespace);
      }
      return paths;
    }

    private void SearchForNextElementInPath(
      ICollection<List<NamespaceName>> paths, 
      List<NamespaceName> currentPath, 
      Pattern toPattern, 
      NamespaceName namespaceName)
    {
      if (currentPath.Count > 0 && namespaceName.Matches(toPattern))
      {
        paths.Add(currentPath.Append(namespaceName).ToList());
        return;
      }

      if (NoDependenciesFrom(namespaceName))
      {
        return;
      }

      foreach (var neighbour in _adjacencyList[namespaceName])
      {
        SearchForNextElementInPath(paths, currentPath.Append(namespaceName).ToList(), toPattern, neighbour);
      }
    }

    private void SearchForNextElementInCycle(
      List<List<NamespaceName>> cycles, 
      NamespaceName namespaceName, 
      CurrentPath currentPath)
    {
      if (NoDependenciesFrom(namespaceName))
      {
        return;
      }

      if (currentPath.ConsistsSolelyOf(namespaceName))
      {
        return;
      }

      //full cycle detected
      if (currentPath.BeginsWith(namespaceName))
      {
        //full cycle is a cycle that starts with current namespace, e.g. A->B->A
        if (CycleIsNotReportedAlreadyAsStartingFromDifferentElement(currentPath, cycles))
        {
          cycles.Add(currentPath.Plus(namespaceName).AsList());
        }

        return;
      }

      //overgrown cycle detected:
      if(currentPath.ContainsButDoesNotBeginWith(namespaceName))
      {
        //overgrown cycles are paths that contains other cycles, e.g. A->B->C->B   
        return;
      }

      foreach (var neighbour in NeighborsOf(namespaceName))
      {
        SearchForNextElementInCycle(cycles, neighbour, currentPath.Plus(namespaceName));
      }
    }

    private List<NamespaceName> NeighborsOf(NamespaceName namespaceName)
    {
      return _adjacencyList[namespaceName];
    }

    private bool NoDependenciesFrom(NamespaceName namespaceName)
    {
      return !_adjacencyList.ContainsKey(namespaceName);
    }

    private static bool CycleIsNotReportedAlreadyAsStartingFromDifferentElement(
      CurrentPath currentPath,
      List<List<NamespaceName>> cycles)
    {
      //A->B->A and B->A->B are the same cycle, no need to report twice
      return !cycles.Any(cycle => 
        cycle
          .Distinct()
          .OrderBy(s => s.Value)
          .SequenceEqual(
            currentPath.ElementsOrderedForEquivalencyComparison()));
    }
  }

  //bug morph into full type 

  public static class KeyValuePairDeconstruction
  {
    public static void Deconstruct<T1, T2>(this KeyValuePair<T1, T2> tuple, out T1 key, out T2 value)
    {
      key = tuple.Key;
      value = tuple.Value;
    }
  }
}
