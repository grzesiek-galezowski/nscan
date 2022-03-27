using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRules;

public class DependencyPathRuleViolationFactory : IDependencyPathRuleViolationFactory
{
  //bug UT
  private readonly IDependencyPathReportFragmentsFormat _reportFragmentsFormat;

  public DependencyPathRuleViolationFactory(IDependencyPathReportFragmentsFormat reportFragmentsFormat)
  {
    _reportFragmentsFormat = reportFragmentsFormat;
  }

  public RuleViolation PathRuleViolation(RuleDescription description,
    IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath)
  {
    return RuleViolation.Create(
      description, 
      "Violating path: ", 
      _reportFragmentsFormat.ApplyToPath(violationPath));
  }
}