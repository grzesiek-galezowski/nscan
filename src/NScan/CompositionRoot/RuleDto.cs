using GlobExpressions;

namespace TddXt.NScan.CompositionRoot
{
  public class RuleDto
  {
    public Glob DependingPattern { get; set; }
    public string RuleName { get; set; }
    public Glob DependencyPattern { get; set; }
    public string DependencyType { get; set; }
  }
}