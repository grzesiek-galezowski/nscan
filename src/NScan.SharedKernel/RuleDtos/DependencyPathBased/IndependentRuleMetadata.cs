using GlobExpressions;
using NScan.Lib;

namespace NScan.SharedKernel.RuleDtos.DependencyPathBased
{
  public static class IndependentRuleMetadata
  {
    public const string IndependentOf = "independentOf";

    public static RuleDescription Format(IndependentRuleComplementDto dto)
    {
      return FormatIndependentRule(
        dto.DependingPattern.Text(), 
        dto.RuleName, 
        dto.DependencyType, 
        dto.DependencyPattern.Pattern);
    }

    private static RuleDescription FormatIndependentRule(string projectAssemblyNamePattern, string ruleName, string dependencyType, string dependencyPattern)
    {
      return new RuleDescription($"{projectAssemblyNamePattern} {ruleName} {dependencyType}:{dependencyPattern}");
    }

    public static RuleDescription FormatIndependentRule(
      Pattern dependingNamePattern,
      string dependencyType, 
      Glob dependencyNamePattern)
    {
      return FormatIndependentRule(
        dependingNamePattern.Text(), 
        IndependentOf, 
        dependencyType, 
        dependencyNamePattern.Pattern);
    }
  }
}
