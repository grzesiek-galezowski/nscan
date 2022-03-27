namespace NScan.SharedKernel.RuleDtos.NamespaceBased;

public static class HasNoUsingsRuleMetadata
{
  public const string HasNoUsings = "hasNoUsings";

  public static RuleDescription Format(NoUsingsRuleComplementDto noUsingsRuleComplementDto)
  {
    return new RuleDescription(
      $"{noUsingsRuleComplementDto.ProjectAssemblyNamePattern.Text()} " +
      $"{noUsingsRuleComplementDto.RuleName} " +
      $"from {noUsingsRuleComplementDto.FromPattern.Text()} " +
      $"to {noUsingsRuleComplementDto.ToPattern.Text()}");
  }
}