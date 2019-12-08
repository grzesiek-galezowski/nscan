namespace NScan.SharedKernel.RuleDtos.NamespaceBased
{
  //TODO consider pushing these members to DTO itself?
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