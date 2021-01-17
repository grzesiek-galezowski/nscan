namespace NScan.SharedKernel.RuleDtos.ProjectScoped
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
