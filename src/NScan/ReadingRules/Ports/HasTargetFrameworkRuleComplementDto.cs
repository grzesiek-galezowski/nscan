using System;

namespace TddXt.NScan.ReadingRules.Ports
{
  public class HasTargetFrameworkRuleComplementDto
  {
    public HasTargetFrameworkRuleComplementDto(Pattern dependingPattern)
    {
      ProjectAssemblyNamePattern = dependingPattern;
    }

    public string RuleName { get; } = RuleNames.HasTargetFramework;
    public Pattern ProjectAssemblyNamePattern { get; }
  }
}