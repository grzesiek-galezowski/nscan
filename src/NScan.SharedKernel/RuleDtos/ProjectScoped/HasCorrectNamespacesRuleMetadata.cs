namespace NScan.SharedKernel.RuleDtos.ProjectScoped
{
  public static class HasCorrectNamespacesRuleMetadata
  {
    public const string HasCorrectNamespaces = "hasCorrectNamespaces";

    public static RuleDescription Format(CorrectNamespacesRuleComplementDto ruleDto)
    {
      return new RuleDescription($"{ruleDto.ProjectAssemblyNamePattern.Text()} {ruleDto.RuleName}");
    }
  }
}
