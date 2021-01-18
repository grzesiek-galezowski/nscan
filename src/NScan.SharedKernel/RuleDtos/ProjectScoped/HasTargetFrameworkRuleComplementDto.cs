using NScan.Lib;

namespace NScan.SharedKernel.RuleDtos.ProjectScoped
{
  public record HasTargetFrameworkRuleComplementDto(
    Pattern ProjectAssemblyNamePattern, 
    string TargetFramework)
  {
    public string RuleName { get; } = HasTargetFrameworkRuleMetadata.HasTargetFramework;
  }
}
