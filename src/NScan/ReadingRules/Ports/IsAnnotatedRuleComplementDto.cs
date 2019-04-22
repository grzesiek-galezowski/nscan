namespace TddXt.NScan.ReadingRules.Ports
{
  public class IsAnnotatedRuleComplementDto
  {
    public string RuleName => RuleNames.HasAnnotationsOn;
    public Pattern ProjectAssemblyNamePattern { get; set; }
    public Pattern ClassNameInclusionPattern { get; set; }
    public Pattern MethodNameInclusionPattern { get; set; }
  }
}