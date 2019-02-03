using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.DependencyPathBasedRules
{
  public class IndependentRule : IDependencyRule
  {
    private readonly IDescribedDependencyCondition _condition;
    private readonly Pattern _dependingAssemblyNamePattern;
    private readonly IRuleViolationFactory _ruleViolationFactory;

    public IndependentRule(
      IDescribedDependencyCondition dependencyCondition, 
      Pattern dependingAssemblyNamePattern,
      IRuleViolationFactory ruleViolationFactory)
    {
      _condition = dependencyCondition;
      _dependingAssemblyNamePattern = dependingAssemblyNamePattern;
      _ruleViolationFactory = ruleViolationFactory;
    }

    public void Check(IAnalysisReportInProgress report, IProjectDependencyPath dependencyPath)
    {
      var dependingAssembly = dependencyPath.AssemblyWithNameMatching(_dependingAssemblyNamePattern);

      if (dependingAssembly.Exists())
      {
        var dependencyAssembly = dependencyPath.AssemblyMatching(_condition, dependingAssembly);
        if (dependencyAssembly.IsNotBefore(dependingAssembly))
        {
          var pathRuleViolation = _ruleViolationFactory.PathRuleViolation(
            _condition.Description(), 
            dependencyPath.SegmentBetween(dependingAssembly, dependencyAssembly));
          report.Add(pathRuleViolation);
        }
      }
      report.FinishedChecking(_condition.Description());
    }
  }


}