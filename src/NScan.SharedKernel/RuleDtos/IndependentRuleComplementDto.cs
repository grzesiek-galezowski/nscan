using GlobExpressions;
using NScan.Lib;
using NScan.SharedKernel.SharedKernel;

namespace NScan.SharedKernel.Ports
{
  public class IndependentRuleComplementDto
  {
    public IndependentRuleComplementDto(string dependencyType, Pattern dependingPattern, Glob dependencyPattern)
    {
      DependencyPattern = dependencyPattern;
      DependencyType = dependencyType;
      DependingPattern = dependingPattern;
    }

    public Glob DependencyPattern { get; } //Glob instead of Pattern because this does not support exclusions
    public string DependencyType { get; }
    public string RuleName { get; } = RuleNames.IndependentOf;
    public Pattern DependingPattern { get; }
  }
}