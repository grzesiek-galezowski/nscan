using GlobExpressions;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.SharedKernel
{
  public static class DependencyDescriptions //BUG what is the point of this class?
  {
    public static string Description(
      Pattern dependingNamePattern,
      string dependencyType, 
      Glob dependencyNamePattern)
    {
      return RuleFormats.FormatIndependentRule(dependingNamePattern.Description(), RuleNames.IndependentOf, dependencyType,
        dependencyNamePattern.Pattern);
    }
  }
}