using System.Collections.Generic;
using NScan.DependencyPathBasedRules;
using NScan.SharedKernel;

namespace NScan.Domain.Root
{
  public class DependencyPathRuleViolationFactory : IDependencyPathRuleViolationFactory
  {
    //bug UT
    private readonly IReportFragmentsFormat _reportFragmentsFormat;

    public DependencyPathRuleViolationFactory(IReportFragmentsFormat reportFragmentsFormat)
    {
      _reportFragmentsFormat = reportFragmentsFormat;
    }

    public RuleViolation PathRuleViolation(string ruleDescription, IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath)
    {
      return new RuleViolation(ruleDescription, "Violating path: ", _reportFragmentsFormat.ApplyToPath(violationPath));
    }
  }
}