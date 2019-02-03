using System;
using System.Collections.Generic;
using TddXt.NScan.Domain.DependencyPathBasedRules;

namespace TddXt.NScan.Domain.SharedKernel
{
  public interface IRuleViolationFactory
  {
    RuleViolation NoCyclesRuleViolation(string ruleDescription, string projectAssemblyName, IReadOnlyList<IReadOnlyList<string>> cycles);
    RuleViolation PathRuleViolation(string ruleDescription, IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath);
    RuleViolation ProjectScopedRuleViolation(string ruleDescription, string violationDescription);
  }

  public class RuleViolationFactory : IRuleViolationFactory
  {
    private readonly IReportFragmentsFormat _reportFragmentsFormat;

    public RuleViolationFactory(IReportFragmentsFormat reportFragmentsFormat)
    {
      _reportFragmentsFormat = reportFragmentsFormat;
    }

    public RuleViolation NoCyclesRuleViolation(string ruleDescription, string projectAssemblyName, IReadOnlyList<IReadOnlyList<string>> cycles)
    {
      return new RuleViolation(ruleDescription, $"Discovered cycle(s) in project {projectAssemblyName}:{Environment.NewLine}", _reportFragmentsFormat.ApplyToCycles(cycles));
    }

    public RuleViolation PathRuleViolation(string ruleDescription, IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath)
    {
      return new RuleViolation(ruleDescription, "Violating path: ", _reportFragmentsFormat.ApplyToPath(violationPath));
    }

    public RuleViolation ProjectScopedRuleViolation(string ruleDescription, string violationDescription)
    {
      return new RuleViolation(ruleDescription, string.Empty, violationDescription);
    }
  }
}