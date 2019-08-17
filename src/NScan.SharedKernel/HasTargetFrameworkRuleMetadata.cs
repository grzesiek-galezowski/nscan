using NScan.SharedKernel.RuleDtos;

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