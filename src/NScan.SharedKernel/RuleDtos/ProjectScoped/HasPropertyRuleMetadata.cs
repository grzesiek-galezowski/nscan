namespace NScan.SharedKernel.RuleDtos.ProjectScoped;

public static class HasPropertyRuleMetadata
{
  public const string HasProperty = "hasProperty";

  public static RuleDescription Format(HasPropertyRuleComplementDto ruleDto)
  {
    return new RuleDescription(
      $"{ruleDto.ProjectAssemblyNamePattern.Text()} " +
      $"{HasProperty} " +
      $"{ruleDto.PropertyName} " +
      $"{ruleDto.PropertyValue.Text()}");
  }
}