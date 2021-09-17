﻿using System;
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

    public RuleViolation NoCyclesRuleViolation(
      string ruleDescription,
      string projectAssemblyName,
      IReadOnlyList<NamespaceDependencyPath> cycles)
    {
      return RuleViolation.Create(
        new RuleDescription(ruleDescription), 
        $"Discovered cycle(s) in project {projectAssemblyName}:{Environment.NewLine}", 
        _reportFragmentsFormat.ApplyTo(cycles, "Cycle"));
    }

    public RuleViolation NoUsingsRuleViolation(string ruleDescription,
      string projectAssemblyName,
      IReadOnlyList<NamespaceDependencyPath> pathsFound)
    {
      return RuleViolation.Create(
        new RuleDescription(ruleDescription), 
        $"Discovered violation(s) in project {projectAssemblyName}:{Environment.NewLine}", 
        _reportFragmentsFormat.ApplyTo(pathsFound, "Violation"));
    }
  }
}
