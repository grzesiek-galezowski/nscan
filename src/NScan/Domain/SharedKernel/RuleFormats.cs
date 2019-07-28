using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.SharedKernel
{
  public static class RuleFormats //bug add special UTs for this extracted code
  {
    public static string Format(HasAttributesOnRuleComplementDto dto)
    {
      return FormatHasAttributesOnRule(
        dto.ProjectAssemblyNamePattern.Description(), 
        dto.RuleName, 
        dto.ClassNameInclusionPattern.Description(), 
        dto.MethodNameInclusionPattern.Description());
    }

    private static string FormatHasAttributesOnRule(
      string projectAssemblyName, 
      string ruleName, 
      string classNameInclusionPattern, 
      string methodNameInclusionPattern)
    {
      return $"{projectAssemblyName} {ruleName} {classNameInclusionPattern}:{methodNameInclusionPattern}";
    }

    public static string Format(NoCircularUsingsRuleComplementDto dto)
    {
      return FormatNoCircularUsingsRule(dto.ProjectAssemblyNamePattern.Description(), dto.RuleName);
    }

    private static string FormatNoCircularUsingsRule(string projectAssemblyNamePattern, string ruleName)
    {
      return $"{projectAssemblyNamePattern} {ruleName}";
    }

    public static string Format(CorrectNamespacesRuleComplementDto dto)
    {
      return FormatCorrectNamespacesRule(dto.ProjectAssemblyNamePattern.Description(), dto.RuleName);
    }

    private static string FormatCorrectNamespacesRule(string projectAssemblyNamePattern, string ruleName)
    {
      return $"{projectAssemblyNamePattern} {ruleName}";
    }

    public static string Format(IndependentRuleComplementDto dto)
    {
      return FormatIndependentRule(dto.DependingPattern.Description(), dto.RuleName, dto.DependencyType, dto.DependencyPattern.Pattern);
    }

    public static string FormatIndependentRule(string projectAssemblyNamePattern, string ruleName, string dependencyType, string dependencyPattern)
    {
      return $"{projectAssemblyNamePattern} {ruleName} {dependencyType}:{dependencyPattern}";
    }
  }
}