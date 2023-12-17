using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRules;

public class IndependentRule(
  IDescribedDependencyCondition dependencyCondition,
  Pattern dependingAssemblyNamePattern,
  IDependencyPathRuleViolationFactory ruleViolationFactory)
  : IDependencyRule
{
  public void Check(IAnalysisReportInProgress report, IProjectDependencyPath dependencyPath)
  {
    var dependingAssembly = dependencyPath.AssemblyWithNameMatching(dependingAssemblyNamePattern);

    if (dependingAssembly.Exists())
    {
      var dependencyAssembly = dependencyPath.AssemblyMatching(dependencyCondition, dependingAssembly);
      if (dependencyAssembly.IsNotBefore(dependingAssembly))
      {
        var pathRuleViolation = ruleViolationFactory.PathRuleViolation(
          dependencyCondition.Description(), 
          dependencyPath.SegmentBetween(dependingAssembly, dependencyAssembly));
        report.Add(pathRuleViolation);
      }
    }
    report.FinishedEvaluatingRule(dependencyCondition.Description());
  }

}
