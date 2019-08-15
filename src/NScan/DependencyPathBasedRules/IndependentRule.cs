using NScan.Lib;
using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.DependencyPathBasedRules
{
  public class IndependentRule : IDependencyRule
  {
    private readonly IDescribedDependencyCondition _condition;
    private readonly Pattern _dependingAssemblyNamePattern;
    private readonly IDependencyPathRuleViolationFactory _ruleViolationFactory;

    public IndependentRule(
      IDescribedDependencyCondition dependencyCondition, 
      Pattern dependingAssemblyNamePattern,
      IDependencyPathRuleViolationFactory ruleViolationFactory)
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