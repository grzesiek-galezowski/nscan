using NScan.Lib;

namespace NScan.SharedKernel.RuleDtos
{
  public class HasTargetFrameworkRuleComplementDto
  {
    public HasTargetFrameworkRuleComplementDto(Pattern dependingPattern, string targetFramework)
    {
      ProjectAssemblyNamePattern = dependingPattern;
      TargetFramework = targetFramework;
    }

    public string RuleName { get; } = HasTargetFrameworkRuleMetadata.HasTargetFramework;
    public Pattern ProjectAssemblyNamePattern { get; }
    public string TargetFramework { get; }
  }
}