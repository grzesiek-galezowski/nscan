namespace TddXt.NScan.ReadingRules.Ports
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