namespace NScan.SharedKernel.RuleDtos.ProjectScoped
{
  public static class HasAttributesOnRuleMetadata
  {
    public const string HasAttributesOn = "hasAttributesOn";

    public static string Format(HasAttributesOnRuleComplementDto dto)
    {
      var projectAssemblyName = dto.ProjectAssemblyNamePattern.Description();
      var classNameInclusionPattern = dto.ClassNameInclusionPattern.Description();
      var methodNameInclusionPattern = dto.MethodNameInclusionPattern.Description();
      return $"{projectAssemblyName} {dto.RuleName} {classNameInclusionPattern}:{methodNameInclusionPattern}";
    }
  }
}