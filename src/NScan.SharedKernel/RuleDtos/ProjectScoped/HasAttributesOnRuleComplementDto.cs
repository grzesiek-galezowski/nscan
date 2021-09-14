using NScan.Lib;

namespace NScan.SharedKernel.RuleDtos.ProjectScoped
{
  public sealed record HasAttributesOnRuleComplementDto(
      Pattern ProjectAssemblyNamePattern, 
      Pattern ClassNameInclusionPattern, 
      Pattern MethodNameInclusionPattern)
  {
    public string RuleName => HasAttributesOnRuleMetadata.HasAttributesOn;
  }
}
