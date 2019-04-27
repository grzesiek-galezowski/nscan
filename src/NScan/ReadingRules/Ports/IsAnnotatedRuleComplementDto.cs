namespace TddXt.NScan.ReadingRules.Ports
{
  public class IsAnnotatedRuleComplementDto
  {
    public string RuleName => RuleNames.HasAttributesOn;
    public Pattern ProjectAssemblyNamePattern { get; set; }
    public Pattern ClassNameInclusionPattern { get; set; }
    public Pattern MethodNameInclusionPattern { get; set; }
  }
}