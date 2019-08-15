using NScan.Lib;
using NScan.SharedKernel.SharedKernel;

namespace NScan.SharedKernel.Ports
{
  public class NoCircularUsingsRuleComplementDto
  {
    public NoCircularUsingsRuleComplementDto(Pattern projectAssemblyNamePattern)
    {
      ProjectAssemblyNamePattern = projectAssemblyNamePattern;
    }

    public string RuleName { get; } = RuleNames.HasNoCircularUsings;
    public Pattern ProjectAssemblyNamePattern { get; }
  }
}