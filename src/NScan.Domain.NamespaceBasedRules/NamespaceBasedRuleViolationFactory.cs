using System;
using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules
{
  public class NamespaceBasedRuleViolationFactory : INamespaceBasedRuleViolationFactory
  {
    private readonly INamespaceBasedReportFragmentsFormat _reportFragmentsFormat;

    public NamespaceBasedRuleViolationFactory(INamespaceBasedReportFragmentsFormat reportFragmentsFormat)
    {
      _reportFragmentsFormat = reportFragmentsFormat;
    }

    public RuleViolation NoCyclesRuleViolation(string ruleDescription, string projectAssemblyName, IReadOnlyList<IReadOnlyList<string>> cycles)
    {
      return new RuleViolation(
        ruleDescription, 
        $"Discovered cycle(s) in project {projectAssemblyName}:{Environment.NewLine}", 
        _reportFragmentsFormat.ApplyTo(cycles, "Cycle"));
    }

    public RuleViolation NoUsingsRuleViolation(string ruleDescription, string projectAssemblyName, IReadOnlyList<IReadOnlyList<string>> pathsFound)
    {
      return new RuleViolation(
        ruleDescription, 
        $"Discovered violation(s) in project {projectAssemblyName}:{Environment.NewLine}", 
        _reportFragmentsFormat.ApplyTo(pathsFound, "Violation"));
    }
  }
}