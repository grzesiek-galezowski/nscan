namespace NScan.SharedKernel.RuleDtos.NamespaceBased
{
  public static class HasNoUsingsRuleMetadata
  {
    public const string HasNoUsings = "hasNoUsings";

    public static string Format(NoUsingsRuleComplementDto dto)
    {
      return $"{dto.ProjectAssemblyNamePattern.Description()} " +
             $"{dto.RuleName} from {dto.FromPattern.Description()} to {dto.ToPattern.Description()}";
    }
  }
}
