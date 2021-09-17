namespace NScan.SharedKernel.RuleDtos.NamespaceBased
{
  public static class HasNoCircularUsingsRuleMetadata
  {
    public const string HasNoCircularUsings = "hasNoCircularUsings";

    public static RuleDescription Format(NoCircularUsingsRuleComplementDto noCircularUsingsRuleComplementDto)
    {
      return new RuleDescription(
        $"{noCircularUsingsRuleComplementDto.ProjectAssemblyNamePattern.Description()} " +
        $"{noCircularUsingsRuleComplementDto.RuleName}");
    }
  }
}
