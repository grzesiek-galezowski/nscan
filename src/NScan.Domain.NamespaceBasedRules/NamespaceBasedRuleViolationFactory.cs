using System;
using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules
{
  public class NamespaceBasedRuleViolationFactory : INamespaceBasedRuleViolationFactory
  {
    //bug UT
    private readonly INamespaceBasedReportFragmentsFormat _reportFragmentsFormat;

    public NamespaceBasedRuleViolationFactory(INamespaceBasedReportFragmentsFormat reportFragmentsFormat)
    {
      _reportFragmentsFormat = reportFragmentsFormat;
    }

    public RuleViolation NoCyclesRuleViolation(string ruleDescription, string projectAssemblyName, IReadOnlyList<IReadOnlyList<string>> cycles)
    {
      return new RuleViolation(ruleDescription, $"Discovered cycle(s) in project {projectAssemblyName}:{Environment.NewLine}", _reportFragmentsFormat.ApplyToCycles(cycles));
    }
  }
}