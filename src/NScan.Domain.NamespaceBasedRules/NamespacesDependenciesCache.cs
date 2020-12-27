using System.Collections.Generic;
using System.Linq;
using NScan.Lib;

namespace NScan.NamespaceBasedRules
{
  public class NamespacesDependenciesCache : INamespacesDependenciesCache
  {
    private readonly Dictionary<NamespaceName, List<NamespaceName>> _adjacencyList = new();

    //bug adjacency list accepts duplicates but should not!
    public void AddMapping(string namespaceName, string usingName)
    {
      InitializeNeighborsOf(new NamespaceName(namespaceName));
      AddNeighborOf(new NamespaceName(namespaceName), new NamespaceName(usingName));
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

    public IReadOnlyList<IReadOnlyList<string>> RetrieveCycles()
    {
      var cycles = new List<List<NamespaceName>>();
      foreach (var @namespace in _adjacencyList.Keys)
      {
        SearchForNextElementInCycle(cycles, @namespace, new List<NamespaceName>());
      }
      return cycles.Select(p => p.Select(e => e.Value).ToList()).ToList();
    }

    public IReadOnlyList<IReadOnlyList<string>> RetrievePathsBetween(Pattern fromPattern, Pattern toPattern)
    {
      var paths = new List<List<NamespaceName>>();
      foreach (var @namespace in _adjacencyList.Keys.Where(k => k.Matches(fromPattern)))
      {
        SearchForNextElementInPath(
          paths, 
          new List<NamespaceName>(), 
          toPattern, @namespace);
      }
      return paths.Select(p => p.Select(e => e.Value).ToList()).ToList();
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

      if (PathEnd(namespaceName))
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
      List<NamespaceName> currentPath)
    {
      if (PathEnd(namespaceName))
      {
        return;
      }

      if (SelfUsingFound(namespaceName, currentPath))
      {
        return;
      }

      if (FullCycleDetected(namespaceName, currentPath))
      {
        //full cycle is a cycle that starts with current namespace, e.g. A->B->A
        if (CycleIsNotReportedAlreadyAsStartingFromDifferentElement(currentPath, cycles))
        {
          cycles.Add(currentPath.Append(namespaceName).ToList());
        }

        return;
      }

      if(OvergrownCycleDetected(namespaceName, currentPath))
      {
        //overgrown cycles are paths that contains other cycles, e.g. A->B->C->B   
        return;
      }

      foreach (var neighbour in NeighborsOf(namespaceName))
      {
        SearchForNextElementInCycle(cycles, neighbour, currentPath.Append(namespaceName).ToList());
      }
    }

    private List<NamespaceName> NeighborsOf(NamespaceName namespaceName)
    {
      return _adjacencyList[namespaceName];
    }

    private static bool SelfUsingFound(NamespaceName namespaceName, List<NamespaceName> currentPath)
    {
      return currentPath.Count == 1 && currentPath[0] == namespaceName;
    }

    private bool PathEnd(NamespaceName namespaceName)
    {
      return !_adjacencyList.ContainsKey(namespaceName);
    }

    private static bool FullCycleDetected(NamespaceName namespaceName, List<NamespaceName> currentPath)
    {
      return currentPath.Contains(namespaceName) && currentPath[0] == namespaceName;
    }

    private bool OvergrownCycleDetected(NamespaceName namespaceName, List<NamespaceName> currentPath)
    {
      return currentPath.Contains(namespaceName) && currentPath[0] != namespaceName;
    }


    private static bool CycleIsNotReportedAlreadyAsStartingFromDifferentElement(IEnumerable<NamespaceName> currentPath, List<List<NamespaceName>> cycles)
    {
      //A->B->A and B->A->B are the same cycle, no need to report twice
      return !cycles.Any(cycle => 
        cycle
          .Distinct()
          .OrderBy(s => s.Value)
          .SequenceEqual(
            currentPath
              .Distinct()
              .OrderBy(s => s.Value)));
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

  public record NamespaceName(string Value)
  {
    public bool Matches(Pattern fromPattern)
    {
      return fromPattern.IsMatch(Value);
    }
  }
}
