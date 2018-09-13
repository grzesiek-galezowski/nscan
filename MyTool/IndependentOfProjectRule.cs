using System;
using System.Collections.Generic;
using System.Linq;
using MyTool.App;
using static MyTool.SearchResult;

namespace MyTool
{
  public class IndependentOfProjectRule : IDependencyRule
  {
    private readonly string _dependingAssemblyName;
    private readonly string _dependencyAssemblyName;

    public IndependentOfProjectRule(string dependingAssemblyName, string dependencyAssemblyName)
    {
      _dependingAssemblyName = dependingAssemblyName;
      _dependencyAssemblyName = dependencyAssemblyName;
    }

    public void Check(IReadOnlyList<IReferencedProject> path, IAnalysisReportInProgress report)
    {
      var depending = Find(path, AssemblyNameMatches(_dependingAssemblyName));

      if (depending.Found)
      {
        var dependency = Find(path, NextAssemblyNameMatches(_dependencyAssemblyName, depending));

        if (dependency.Found && depending.IsBefore(dependency))
        {
          report.ViolationOf(
            DependencyDescriptions.IndependentOf(_dependingAssemblyName, _dependencyAssemblyName),
            depending.SegmentEndingWith(dependency, path));
        }
        else
        {
          report.Ok(
            DependencyDescriptions.IndependentOf(_dependingAssemblyName, _dependencyAssemblyName));
        }
      }
      else
      {
        report.Ok(
          DependencyDescriptions.IndependentOf(_dependingAssemblyName, _dependencyAssemblyName));
      }
    }

    private Func<IReferencedProject, bool> NextAssemblyNameMatches(string dependencyAssemblyPattern, SearchResult<IReferencedProject> depending)
    {
      return p => AssemblyNameMatches(dependencyAssemblyPattern)(p) && !depending.Value.Equals(p);
    }

    private SearchResult<IReferencedProject> Find(
      IReadOnlyList<IReferencedProject> path, 
      Func<IReferencedProject, bool> assemblyNameMatches)
    {
      if (path.Any(assemblyNameMatches))
      {
        return path
          .Select(ItemFound)
          .First(p => assemblyNameMatches(p.Value));
      }
      else
      {
        return ItemNotFound();
      }
    }

    private static Func<IReferencedProject, bool> AssemblyNameMatches(string assemblyNamePattern)
    {
      return p => p.HasAssemblyNameMatching(assemblyNamePattern);
    }
  }

  internal class SearchResult<T>
  {
    private readonly T _instance;
    private readonly int _index; 

    public SearchResult(T instance, int index)
    {
      _instance = instance;
      _index = index;
    }

    public bool Found  => _instance != null;
    public T Value => Found ? _instance : throw new NoValueException(typeof(T));

    public List<T> SegmentEndingWith(SearchResult<T> second, IEnumerable<T> path) => path.ToList().GetRange(_index, second._index - _index + 1);

    public bool IsBefore(SearchResult<T> dependency)
    {
      return _index < dependency._index;
    }
  }

  internal static class SearchResult
  {
    public static SearchResult<T> ItemFound<T>(T instance, int i)
    {
      return new SearchResult<T>(instance, i);
    }

    public static SearchResult<IReferencedProject> ItemNotFound()
    {
      return new SearchResult<IReferencedProject>(null, 0);
    }
  }

  public class NoValueException : Exception
  {
    public NoValueException(Type type) : base("No value of type " + type + " in search result")
    {
      
    }
  }
}