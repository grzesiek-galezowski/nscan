using GlobExpressions;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.SharedKernel
{
  public static class RuleFormats
  {
    public static string Format(HasAttributesOnRuleComplementDto dto)
    {
      var projectAssemblyName = dto.ProjectAssemblyNamePattern.Description();
      var classNameInclusionPattern = dto.ClassNameInclusionPattern.Description();
      var methodNameInclusionPattern = dto.MethodNameInclusionPattern.Description();
      return $"{projectAssemblyName} {dto.RuleName} {classNameInclusionPattern}:{methodNameInclusionPattern}";
    }

    public static string Format(NoCircularUsingsRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}";
    }

    public static string Format(CorrectNamespacesRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}";
    }

    public static string Format(IndependentRuleComplementDto dto)
    {
      return FormatIndependentRule(dto.DependingPattern.Description(), dto.RuleName, dto.DependencyType, dto.DependencyPattern.Pattern);
    }


    public static string Format(HasTargetFrameworkRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {RuleNames.HasTargetFramework} {dto.TargetFramework}";
    }

    public static string FormatIndependentRule(
      Pattern dependingNamePattern,
      string dependencyType, 
      Glob dependencyNamePattern)
    {
      return FormatIndependentRule(dependingNamePattern.Description(), RuleNames.IndependentOf, dependencyType,
        dependencyNamePattern.Pattern);
    }

    private static string FormatIndependentRule(string projectAssemblyNamePattern, string ruleName, string dependencyType, string dependencyPattern)
    {
      return $"{projectAssemblyNamePattern} {ruleName} {dependencyType}:{dependencyPattern}";
    }
  }
}