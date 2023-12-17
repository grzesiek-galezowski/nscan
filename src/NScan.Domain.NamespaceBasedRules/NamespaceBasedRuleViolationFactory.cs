using System;
using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules;

public class NamespaceBasedRuleViolationFactory(INamespaceBasedReportFragmentsFormat reportFragmentsFormat)
  : INamespaceBasedRuleViolationFactory
{
  public RuleViolation NoCyclesRuleViolation(
    RuleDescription description,
    AssemblyName projectAssemblyName,
    IReadOnlyList<NamespaceDependencyPath> cycles)
  {
    return RuleViolation.Create(
      description, 
      $"Discovered cycle(s) in project {projectAssemblyName}:{Environment.NewLine}", 
      reportFragmentsFormat.ApplyTo(cycles, "Cycle"));
  }

  public RuleViolation NoUsingsRuleViolation(
    RuleDescription description,
    AssemblyName projectAssemblyName,
    IReadOnlyList<NamespaceDependencyPath> pathsFound)
  {
    return RuleViolation.Create(
      description, 
      $"Discovered violation(s) in project {projectAssemblyName}:{Environment.NewLine}", 
      reportFragmentsFormat.ApplyTo(pathsFound, "Violation"));
  }
}
