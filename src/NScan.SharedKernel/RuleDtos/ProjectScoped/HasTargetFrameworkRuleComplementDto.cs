using NScan.Lib;

namespace NScan.SharedKernel.RuleDtos.ProjectScoped
{
  public sealed record HasTargetFrameworkRuleComplementDto(
    Pattern ProjectAssemblyNamePattern, 
    string TargetFramework)
  {
    public string RuleName => HasTargetFrameworkRuleMetadata.HasTargetFramework;
  }
}
