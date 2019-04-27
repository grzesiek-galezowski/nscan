namespace TddXt.NScan.ReadingRules.Ports
{
  public class IsAnnotatedRuleComplementDto
  {
    public IsAnnotatedRuleComplementDto(
      Pattern projectAssemblyNamePattern, 
      Pattern classNameInclusionPattern, 
      Pattern methodNameInclusionPattern)
    {
      ProjectAssemblyNamePattern = projectAssemblyNamePattern;
      ClassNameInclusionPattern = classNameInclusionPattern;
      MethodNameInclusionPattern = methodNameInclusionPattern;
    }

    public string RuleName => RuleNames.HasAttributesOn;
    public Pattern ProjectAssemblyNamePattern { get; }
    public Pattern ClassNameInclusionPattern { get; }
    public Pattern MethodNameInclusionPattern { get; }
  }
}