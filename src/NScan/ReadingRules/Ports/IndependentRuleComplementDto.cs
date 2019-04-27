using GlobExpressions;

namespace TddXt.NScan.ReadingRules.Ports
{
  public class IndependentRuleComplementDto
  {
    public IndependentRuleComplementDto(string dependencyType, Pattern dependingPattern, Glob dependencyPattern)
    {
      DependencyPattern = dependencyPattern;
      DependencyType = dependencyType;
      DependingPattern = dependingPattern;
    }

    public Glob DependencyPattern { get; } //bug Pattern instead of Glob?
    public string DependencyType { get; }
    public string RuleName { get; } = RuleNames.IndependentOf;
    public Pattern DependingPattern { get; }
  }
}