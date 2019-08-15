using NScan.Lib;
using NScan.SharedKernel.SharedKernel;

namespace NScan.SharedKernel.Ports
{
  public class CorrectNamespacesRuleComplementDto
  {
    public CorrectNamespacesRuleComplementDto(Pattern projectAssemblyNamePattern)
    {
      ProjectAssemblyNamePattern = projectAssemblyNamePattern;
    }

    public string RuleName { get; } = RuleNames.HasCorrectNamespaces;
    public Pattern ProjectAssemblyNamePattern { get; }
  }
}