namespace TddXt.NScan.ReadingRules.Ports
{
  public class CorrectNamespacesRuleComplementDto
  {
    public string RuleName { get; } = RuleNames.HasCorrectNamespaces;
    public Pattern ProjectAssemblyNamePattern { get; set; }
  }
}