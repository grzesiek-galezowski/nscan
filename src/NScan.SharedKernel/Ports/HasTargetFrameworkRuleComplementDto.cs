using NScan.Lib;
using NScan.SharedKernel.SharedKernel;

namespace NScan.SharedKernel.Ports
{
  public class HasTargetFrameworkRuleComplementDto
  {
    public HasTargetFrameworkRuleComplementDto(Pattern dependingPattern, string targetFramework)
    {
      ProjectAssemblyNamePattern = dependingPattern;
      TargetFramework = targetFramework;
    }

    public string RuleName { get; } = RuleNames.HasTargetFramework;
    public Pattern ProjectAssemblyNamePattern { get; }
    public string TargetFramework { get; }
  }
}