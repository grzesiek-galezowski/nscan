using NScan.Lib;

namespace NScan.SharedKernel.RuleDtos
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