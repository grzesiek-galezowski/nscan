namespace NScan.SharedKernel.RuleDtos.ProjectScoped;

public static class HasAttributesOnRuleMetadata
{
  public const string HasAttributesOn = "hasAttributesOn";

  public static RuleDescription Format(HasAttributesOnRuleComplementDto ruleDto)
  {
    var projectAssemblyName = ruleDto.ProjectAssemblyNamePattern.Text();
    var classNameInclusionPattern = ruleDto.ClassNameInclusionPattern.Text();
    var methodNameInclusionPattern = ruleDto.MethodNameInclusionPattern.Text();
    return new RuleDescription($"{projectAssemblyName} {ruleDto.RuleName} {classNameInclusionPattern}:{methodNameInclusionPattern}");
  }
}