namespace NScan.SharedKernel.RuleDtos.NamespaceBased
{
  public static class HasNoCircularUsingsRuleMetadata
  {
    public const string HasNoCircularUsings = "hasNoCircularUsings";

    public static string Format(NoCircularUsingsRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}";
    }
  }
}