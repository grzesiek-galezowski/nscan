namespace NScan.SharedKernel.RuleDtos.ProjectScoped
{
  public static class HasPropertyRuleMetadata
  {
    public const string HasProperty = "hasProperty";

    public static string Format(HasPropertyRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {HasProperty} {dto.PropertyName} {dto.PropertyValue}";
    }
  }
}
