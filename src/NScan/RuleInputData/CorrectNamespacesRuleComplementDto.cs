using TddXt.NScan.Domain;

namespace TddXt.NScan.RuleInputData
{
  public class CorrectNamespacesRuleComplementDto
  {
    public string RuleName { get; } = RuleNames.HasCorrectNamespaces;
    public Pattern ProjectAssemblyNamePattern { get; set; }
  }

  public class NoCircularUsingsRuleComplementDto
  {
    public string RuleName { get; } = RuleNames.HasNoCircularUsings;
    public Pattern ProjectAssemblyNamePattern { get; set; }
  }
}