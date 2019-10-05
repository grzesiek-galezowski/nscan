using System;
using System.Collections.Generic;
using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;

namespace NScan.Domain.Root
{
  public interface IRuleViolationFactory : INamespaceBasedRuleViolationFactory, IDependencyPathRuleViolationFactory, IProjectScopedRuleViolationFactory
  {}

  public class RuleViolationFactory : IRuleViolationFactory //bug split entirely into three types
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