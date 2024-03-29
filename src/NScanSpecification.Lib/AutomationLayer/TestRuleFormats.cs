﻿using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScanSpecification.Lib.AutomationLayer;

public static class TestRuleFormats
{
  public static string FormatHasAttributesOnRule(
    string projectAssemblyNamePattern,
    string classNameInclusionPattern,
    string methodNameInclusionPattern)
  {
    return $"{projectAssemblyNamePattern} {HasAttributesOnRuleMetadata.HasAttributesOn} {classNameInclusionPattern}:{methodNameInclusionPattern}";
  }

  public static string FormatNoCircularUsingsRule(string projectAssemblyNamePattern)
  {
    return $"{projectAssemblyNamePattern} {HasNoCircularUsingsRuleMetadata.HasNoCircularUsings}";
  }

  public static string FormatNoUsingsRule(string projectAssemblyNamePattern, string from, string to)
  {
    return $"{projectAssemblyNamePattern} hasNoUsings from {from} to {to}";
  }

  public static string FormatCorrectNamespacesRule(string projectAssemblyNamePattern)
  {
    return $"{projectAssemblyNamePattern} {HasCorrectNamespacesRuleMetadata.HasCorrectNamespaces}";
  }

  public static string FormatIndependentRule(string projectAssemblyNamePattern, string dependencyType, string dependencyPattern)
  {
    return $"{projectAssemblyNamePattern} {IndependentRuleMetadata.IndependentOf} {dependencyType}:{dependencyPattern}";
  }

  public static string FormatHasTargetFrameworkRule(
    string projectAssemblyNamePattern,
    string frameworkId)
  {
    return $"{projectAssemblyNamePattern} {HasTargetFrameworkRuleMetadata.HasTargetFramework} {frameworkId}";
  }

  public static string FormatHasPropertyRule(
    string projectAssemblyNamePattern, 
    string propertyName, 
    string propertyValue)
  {
    return $"{projectAssemblyNamePattern} hasProperty {propertyName} {propertyValue}";
  }
}