namespace TddXt.NScan.ReadingRules.Ports
{
  public class NoCircularUsingsRuleComplementDto
  {
    public NoCircularUsingsRuleComplementDto(Pattern projectAssemblyNamePattern)
    {
      ProjectAssemblyNamePattern = projectAssemblyNamePattern;
    }

    public string RuleName { get; } = RuleNames.HasNoCircularUsings;
    public Pattern ProjectAssemblyNamePattern { get; }
  }
}