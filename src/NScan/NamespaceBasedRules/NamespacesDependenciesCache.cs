using System.Collections.Generic;
using System.Linq;

namespace NScan.Domain.Domain.NamespaceBasedRules
{
  public class NamespacesDependenciesCache : INamespacesDependenciesCache
  {
    private readonly Dictionary<string, List<string>> _adjacencyList = new Dictionary<string, List<string>>();

    public void AddMapping(string namespaceName, string usingName)
    {
      if (!_adjacencyList.ContainsKey(namespaceName))
      {
        _adjacencyList[namespaceName] = new List<string>();
      }
      _adjacencyList[namespaceName].Add(usingName);
    }

    public IReadOnlyList<IReadOnlyList<string>> RetrieveCycles()
    {
      var cycles = new List<List<string>>();
      foreach (var @namespace in _adjacencyList.Keys)
      {
        Fill(@namespace, new List<string>(), cycles);
      }
      return cycles;
    }

    private void Fill(string @namespace, List<string> currentPath, List<List<string>> cycles)
    {
      if (PathEndedWithoutACycle(@namespace))
      {
      }
      else if (SelfUsingFound(@namespace, currentPath))
      {

      }
      else if (FullCycleDetected(@namespace, currentPath))
      {
        //full cycle is a cycle that starts with current namespace, e.g. A->B->A
        if (CycleIsNotReportedAlreadyAsStartingFromDifferentElement(currentPath, cycles))
        {
          cycles.Add(currentPath.Append(@namespace).ToList());
        }
      }
      else if(OvergrownCycleDetected(@namespace, currentPath))
      {
        //overgrown cycles are paths that contains other cycles, e.g. A->B->C->B   
      }
      else
      {
        var neighbours = _adjacencyList[@namespace];
        foreach (var neighbour in neighbours)
        {
          Fill(neighbour, currentPath.Append(@namespace).ToList(), cycles);
        }
      }
    }

    private static bool SelfUsingFound(string @namespace, List<string> currentPath)
    {
      return currentPath.Count == 1 && currentPath[0] == @namespace;
    }

    private bool PathEndedWithoutACycle(string @namespace)
    {
      return !_adjacencyList.ContainsKey(@namespace);
    }

    private static bool FullCycleDetected(string @namespace, IReadOnlyList<string> currentPath)
    {
      return currentPath.Contains(@namespace) && currentPath[0] == @namespace;
    }

    private bool OvergrownCycleDetected(string @namespace, List<string> currentPath)
    {
      return currentPath.Contains(@namespace) && currentPath[0] != @namespace;
    }


    private static bool CycleIsNotReportedAlreadyAsStartingFromDifferentElement(IEnumerable<string> currentPath, List<List<string>> cycles)
    {
      //A->B->A and B->A->B are the same cycle, no need to report twice
      return !cycles.Any(cycle => cycle.Distinct().OrderBy(s => s).SequenceEqual(currentPath.Distinct().OrderBy(s => s)));
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