using NScan.Lib;

namespace NScan.SharedKernel.RuleDtos.ProjectScoped;

public sealed record HasPropertyRuleComplementDto(
  Pattern ProjectAssemblyNamePattern, 
  string PropertyName,
  Pattern PropertyValue)
{
  public string RuleName => HasPropertyRuleMetadata.HasProperty;
}