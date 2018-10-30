using System;
using TddXt.NScan.App;

namespace TddXt.NScan
{
  public class IndependentRule : IDependencyRule
  {
    private readonly IDescribedDependencyCondition _condition;
    private readonly Glob.Glob _dependingAssemblyNamePattern;

    public IndependentRule(IDescribedDependencyCondition dependencyCondition, Glob.Glob dependingAssemblyNamePattern)
    {
      _condition = dependencyCondition;
      _dependingAssemblyNamePattern = dependingAssemblyNamePattern;
    }

    public void Check(IAnalysisReportInProgress report, IProjectDependencyPath dependencyPath)
    {
      var dependingAssembly = dependencyPath.AssemblyWithNameMatching(_dependingAssemblyNamePattern);

      if (dependingAssembly.Exists())
      {
        var dependencyAssembly = dependencyPath.AssemblyMatching(_condition, dependingAssembly);
        if (dependencyAssembly.IsNotBefore(dependingAssembly))
        {
          report.Violation(
            _condition.Description(),
            dependencyPath.SegmentBetween(dependingAssembly, dependencyAssembly));
        }
        else
        {
          report.Ok(_condition.Description());
        }
      }
      else
      {
        report.Ok(_condition.Description());
      }
    }
  }


}