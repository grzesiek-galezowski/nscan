using NScan.Lib;
using NScan.SharedKernel.SharedKernel;

namespace NScan.SharedKernel.Ports
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

    public string RuleName => RuleNames.HasAttributesOn;
    public Pattern ProjectAssemblyNamePattern { get; }
    public Pattern ClassNameInclusionPattern { get; }
    public Pattern MethodNameInclusionPattern { get; }
  }
}