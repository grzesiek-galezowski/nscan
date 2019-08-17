using NScan.Lib;

namespace NScan.SharedKernel.RuleDtos
{
  public class HasAttributesOnRuleComplementDto
  {
    public HasAttributesOnRuleComplementDto(
      Pattern projectAssemblyNamePattern, 
      Pattern classNameInclusionPattern, 
      Pattern methodNameInclusionPattern)
    {
      ProjectAssemblyNamePattern = projectAssemblyNamePattern;
      ClassNameInclusionPattern = classNameInclusionPattern;
      MethodNameInclusionPattern = methodNameInclusionPattern;
    }

    public string RuleName => HasAttributesOnRuleMetadata.HasAttributesOn;
    public Pattern ProjectAssemblyNamePattern { get; }
    public Pattern ClassNameInclusionPattern { get; }
    public Pattern MethodNameInclusionPattern { get; }
  }
}