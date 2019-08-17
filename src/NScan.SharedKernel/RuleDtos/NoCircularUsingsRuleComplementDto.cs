using NScan.Lib;

namespace NScan.SharedKernel.RuleDtos
{
  public class NoCircularUsingsRuleComplementDto
  {
    public NoCircularUsingsRuleComplementDto(Pattern projectAssemblyNamePattern)
    {
      ProjectAssemblyNamePattern = projectAssemblyNamePattern;
    }

    public string RuleName { get; } = HasNoCircularUsingsRuleMetadata.HasNoCircularUsings;
    public Pattern ProjectAssemblyNamePattern { get; }
  }
}