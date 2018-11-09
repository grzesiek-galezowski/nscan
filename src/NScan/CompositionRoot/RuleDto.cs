using GlobExpressions;

namespace TddXt.NScan.CompositionRoot
{
  public class RuleDto
  {
    public string RuleName { get; set; }
    public Pattern DependingPattern { get; set; }
    public Glob DependencyPattern { get; set; }
    public string DependencyType { get; set; }

  }
}