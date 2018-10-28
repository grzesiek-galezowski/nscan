using System;
using TddXt.NScan.App;

namespace TddXt.NScan
{
  public class IndependentRule : IDependencyRule
  {
    private readonly string _dependingAssemblyNamePattern;
    private readonly IDependencyCondition _condition;

    public IndependentRule(string dependingAssemblyNamePattern, IDependencyCondition dependencyCondition)
    {
      _dependingAssemblyNamePattern = dependingAssemblyNamePattern;
      _condition = dependencyCondition;
    }

    public void Check(IAnalysisReportInProgress report, IProjectDependencyPath dependencyPath)
    {
      var dependingAssembly = dependencyPath.AssemblyWithNameMatching(_dependingAssemblyNamePattern);

      if (dependingAssembly.Exists())
      {
        var dependencyAssembly = dependencyPath.AssemblyMatching(_condition, dependingAssembly);
        if (dependencyAssembly.ExistsAfter(dependingAssembly))
        {
          report.Violation(
            _condition.Description(_dependingAssemblyNamePattern),
            dependencyPath.SegmentBetween(dependingAssembly, dependencyAssembly));
        }
        else
        {
          report.Ok(_condition.Description(_dependingAssemblyNamePattern));
        }
      }
      else
      {
        report.Ok(_condition.Description(_dependingAssemblyNamePattern));
      }
    }
  }


}