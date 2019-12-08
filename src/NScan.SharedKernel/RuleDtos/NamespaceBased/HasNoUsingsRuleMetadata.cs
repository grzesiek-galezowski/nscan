namespace NScan.SharedKernel.RuleDtos.NamespaceBased
{
  public static class HasNoUsingsRuleMetadata
  {
    //bug UT
    public const string RuleName = "hasNoUsings";

    public static string Format(NoUsingsRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} " +
             $"hasNoUsings from {dto.FromPattern.Description()} to {dto.ToPattern.Description()}";
    }
  }
}