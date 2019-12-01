using NScan.SharedKernel.RuleDtos;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.SharedKernel
{
  public static class HasTargetFrameworkRuleMetadata
  {
    public const string HasTargetFramework = "hasTargetFramework";

    public static string Format(HasTargetFrameworkRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {HasTargetFramework} {dto.TargetFramework}";
    }
  }
}