namespace NScan.SharedKernel.RuleDtos.ProjectScoped
{
  public static class HasAttributesOnRuleMetadata
  {
    public const string HasAttributesOn = "hasAttributesOn";

    public static RuleDescription Format(HasAttributesOnRuleComplementDto ruleDto)
    {
      var projectAssemblyName = ruleDto.ProjectAssemblyNamePattern.Description();
      var classNameInclusionPattern = ruleDto.ClassNameInclusionPattern.Description();
      var methodNameInclusionPattern = ruleDto.MethodNameInclusionPattern.Description();
      return new RuleDescription($"{projectAssemblyName} {ruleDto.RuleName} {classNameInclusionPattern}:{methodNameInclusionPattern}");
    }
  }
}
