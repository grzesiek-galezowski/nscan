using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public static class TestRuleFormats
  {
    public static string FormatHasAttributesOnRule(
      string projectAssemblyNamePattern,
      string classNameInclusionPattern,
      string methodNameInclusionPattern)
    {
      return $"{projectAssemblyNamePattern} {RuleNames.HasAttributesOn} {classNameInclusionPattern}:{methodNameInclusionPattern}";
    }

    public static string FormatNoCircularUsingsRule(string projectAssemblyNamePattern)
    {
      return $"{projectAssemblyNamePattern} {RuleNames.HasNoCircularUsings}";
    }

    public static string FormatCorrectNamespacesRule(string projectAssemblyNamePattern)
    {
      return $"{projectAssemblyNamePattern} {RuleNames.HasCorrectNamespaces}";
    }

    public static string FormatIndependentRule(string projectAssemblyNamePattern, string dependencyType, string dependencyPattern)
    {
      return $"{projectAssemblyNamePattern} {RuleNames.IndependentOf} {dependencyType}:{dependencyPattern}";
    }

    public static string FormatHasTargetFrameworkRule(
      string projectAssemblyNamePattern,
      string frameworkId)
    {
      return $"{projectAssemblyNamePattern} {RuleNames.HasTargetFramework} {frameworkId}";
    }
  }
}