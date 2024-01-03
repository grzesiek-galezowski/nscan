using System;
using LanguageExt;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules;

public class NamespaceBasedRuleViolationFactory(INamespaceBasedReportFragmentsFormat reportFragmentsFormat)
  : INamespaceBasedRuleViolationFactory
{
  public RuleViolation NoCyclesRuleViolation(
    RuleDescription description,
    AssemblyName projectAssemblyName,
    Seq<NamespaceDependencyPath> cycles)
  {
    return RuleViolation.Create(
      description, 
      $"Discovered cycle(s) in project {projectAssemblyName}:{Environment.NewLine}", 
      reportFragmentsFormat.ApplyTo(cycles, "Cycle"));
  }

  public RuleViolation NoUsingsRuleViolation(
    RuleDescription description,
    AssemblyName projectAssemblyName,
    Seq<NamespaceDependencyPath> pathsFound)
  {
    return RuleViolation.Create(
      description, 
      $"Discovered violation(s) in project {projectAssemblyName}:{Environment.NewLine}", 
      reportFragmentsFormat.ApplyTo(pathsFound, "Violation"));
  }
}
