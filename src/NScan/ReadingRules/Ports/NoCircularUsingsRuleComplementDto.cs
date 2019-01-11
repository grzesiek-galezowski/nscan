namespace TddXt.NScan.ReadingRules.Ports
{
  public class NoCircularUsingsRuleComplementDto
  {
    public string RuleName { get; } = RuleNames.HasNoCircularUsings;
    public Pattern ProjectAssemblyNamePattern { get; set; }
  }
}