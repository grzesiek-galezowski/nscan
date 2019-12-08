using System;
using System.Collections.Generic;
using NScan.NamespaceBasedRules;
using NScan.SharedKernel;

namespace NScan.Domain.Root
{
  public class NamespaceBasedRuleViolationFactory : INamespaceBasedRuleViolationFactory
  {
    //bug UT
    private readonly IReportFragmentsFormat _reportFragmentsFormat;

    public NamespaceBasedRuleViolationFactory(IReportFragmentsFormat reportFragmentsFormat)
    {
      _reportFragmentsFormat = reportFragmentsFormat;
    }

    public RuleViolation NoCyclesRuleViolation(string ruleDescription, string projectAssemblyName, IReadOnlyList<IReadOnlyList<string>> cycles)
    {
      return new RuleViolation(ruleDescription, $"Discovered cycle(s) in project {projectAssemblyName}:{Environment.NewLine}", _reportFragmentsFormat.ApplyToCycles(cycles));
    }
  }
}