using System;
using System.Collections.Generic;
using System.Linq;
using TddXt.NScan.App;

namespace TddXt.NScan
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

    private static Func<IReferencedProject, bool> NextAssemblyNameMatches(string dependencyAssemblyPattern, SearchResult<IReferencedProject> depending)
    {
      return p => AssemblyNameMatches(dependencyAssemblyPattern)(p) && !depending.Value.Equals(p);
    }

    private static SearchResult<IReferencedProject> Find(
      IReadOnlyList<IReferencedProject> path, 
      Func<IReferencedProject, bool> assemblyNameMatches)
    {
      if (path.Any(assemblyNameMatches))
      {
        return path
          .Select(SearchResult.ItemFound)
          .First(p => assemblyNameMatches(p.Value));
      }
      else
      {
        return SearchResult.ItemNotFound();
      }
    }

    private static Func<IReferencedProject, bool> AssemblyNameMatches(string assemblyNamePattern)
    {
      return p => p.HasAssemblyNameMatching(assemblyNamePattern);
    }
  }


}