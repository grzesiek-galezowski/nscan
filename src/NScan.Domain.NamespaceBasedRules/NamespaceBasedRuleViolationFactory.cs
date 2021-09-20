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

    public RuleViolation NoCyclesRuleViolation(
      RuleDescription description, //bug remove
      AssemblyName projectAssemblyName,
      IReadOnlyList<NamespaceDependencyPath> cycles)
    {
      return RuleViolation.Create(
        description, 
        $"Discovered cycle(s) in project {projectAssemblyName}:{Environment.NewLine}", 
        _reportFragmentsFormat.ApplyTo(cycles, "Cycle"));
    }

    public RuleViolation NoUsingsRuleViolation(
      RuleDescription description,
      AssemblyName projectAssemblyName,
      IReadOnlyList<NamespaceDependencyPath> pathsFound)
    {
      return RuleViolation.Create(
        description, 
        $"Discovered violation(s) in project {projectAssemblyName}:{Environment.NewLine}", 
        _reportFragmentsFormat.ApplyTo(pathsFound, "Violation"));
    }
  }
}
