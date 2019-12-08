namespace NScan.SharedKernel.RuleDtos.ProjectScoped
{
  public static class HasCorrectNamespacesRuleMetadata
  {
    public const string HasCorrectNamespaces = "hasCorrectNamespaces";

    public static string Format(CorrectNamespacesRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}";
    }

  }
}