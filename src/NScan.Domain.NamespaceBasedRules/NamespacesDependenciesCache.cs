using System.Collections.Generic;
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

    public IReadOnlyList<IReadOnlyList<NamespaceName>> RetrieveCycles()
    {
      //TODO return cycles as well
      var cycles = new List<PotentialCycle>();
      foreach (var @namespace in _dependenciesByNamespace.Keys)
      {
        SearchForNextElementInCycle(
          cycles, 
          @namespace, 
          PotentialCycle.Empty());
      }
      return cycles.Select(c => c.AsList()).ToList(); //bug
    }

    public IReadOnlyList<IReadOnlyList<NamespaceName>> 
      RetrievePathsBetween(Pattern fromPattern, Pattern toPattern)
    {
      var paths = new List<List<NamespaceName>>();
      foreach (var @namespace in _dependenciesByNamespace.Keys.Where(name => name.Matches(fromPattern)))
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

      foreach (var neighbour in _dependenciesByNamespace[namespaceName])
      {
        SearchForNextElementInPath(paths, currentPath.Append(namespaceName).ToList(), toPattern, neighbour);
      }
    }

    private void SearchForNextElementInCycle(
      List<PotentialCycle> cycles, 
      NamespaceName current, 
      PotentialCycle potentialCycle)
    {
      if (NoDependenciesFrom(current))
      {
        return;
      }

      if (potentialCycle.ConsistsSolelyOf(current))
      {
        return;
      }

      //full cycle detected
      if (potentialCycle.BeginsWith(current))
      {
        //full cycle is a cycle that starts with current namespace, e.g. A->B->A
        if (CycleIsNotReportedAlreadyAsStartingFromDifferentElement(potentialCycle, cycles))
        {
          cycles.Add(potentialCycle.Plus(current));
        }

        return;
      }

      //overgrown cycle detected:
      if(potentialCycle.ContainsButDoesNotBeginWith(current))
      {
        //overgrown cycles are paths that contains other cycles, e.g. A->B->C->B   
        return;
      }

      foreach (var neighbour in NeighborsOf(current))
      {
        SearchForNextElementInCycle(cycles, neighbour, potentialCycle.Plus(current));
      }
    }

    private List<NamespaceName> NeighborsOf(NamespaceName namespaceName)
    {
      return _dependenciesByNamespace[namespaceName];
    }

    private bool NoDependenciesFrom(NamespaceName namespaceName)
    {
      return !_dependenciesByNamespace.ContainsKey(namespaceName);
    }

    private static bool CycleIsNotReportedAlreadyAsStartingFromDifferentElement(
      PotentialCycle potentialCycle,
      List<PotentialCycle> cycles)
    {
      //A->B->A and B->A->B are the same cycle, no need to report twice
      return !cycles.Any(c => c.IsEquivalentTo(potentialCycle));
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
