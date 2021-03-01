using NScan.Lib;

namespace NScan.SharedKernel.RuleDtos.ProjectScoped
{
  public class HasPropertyRuleComplementDto
  {
    public HasPropertyRuleComplementDto(Pattern dependingPattern, string propertyName, Pattern propertyValue)
    {
      PropertyName = propertyName;
      PropertyValue = propertyValue;
      ProjectAssemblyNamePattern = dependingPattern;
    }

    public string RuleName { get; } = HasPropertyRuleMetadata.HasProperty;
    public Pattern ProjectAssemblyNamePattern { get; }
    public string PropertyName { get; }
    public Pattern PropertyValue { get; }
  }
}
