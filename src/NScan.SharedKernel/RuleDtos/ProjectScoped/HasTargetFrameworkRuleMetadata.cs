namespace NScan.SharedKernel.RuleDtos.ProjectScoped
{
  public static class HasTargetFrameworkRuleMetadata
  {
    public const string HasTargetFramework = "hasTargetFramework";

    public static RuleDescription Format(HasTargetFrameworkRuleComplementDto ruleDto)
    {
      return new RuleDescription(
        $"{ruleDto.ProjectAssemblyNamePattern.Description()} " +
        $"{HasTargetFramework} " +
        $"{ruleDto.TargetFramework}");
    }
  }
}
