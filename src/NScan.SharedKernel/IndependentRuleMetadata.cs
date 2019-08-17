using GlobExpressions;
using NScan.Lib;
using NScan.SharedKernel.RuleDtos;

namespace NScan.SharedKernel
{
  public static class IndependentRuleMetadata
  {
    public const string IndependentOf = "independentOf";

    public static string Format(IndependentRuleComplementDto dto)
    {
      return FormatIndependentRule(dto.DependingPattern.Description(), dto.RuleName, dto.DependencyType, dto.DependencyPattern.Pattern);
    }

    public static string FormatIndependentRule(
      Pattern dependingNamePattern,
      string dependencyType, 
      Glob dependencyNamePattern)
    {
      return FormatIndependentRule(dependingNamePattern.Description(), IndependentRuleMetadata.IndependentOf, dependencyType,
        dependencyNamePattern.Pattern);
    }

    private static string FormatIndependentRule(string projectAssemblyNamePattern, string ruleName, string dependencyType, string dependencyPattern)
    {
      return $"{projectAssemblyNamePattern} {ruleName} {dependencyType}:{dependencyPattern}";
    }
  }
}