using GlobExpressions;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.SharedKernel
{
  public static class DependencyDescriptions
  {
    public static string IndependentOf(string dependingAssemblyName, string dependencyAssemblyName)
    {
      return "[" + dependingAssemblyName + "] independentOf [" + dependencyAssemblyName + "]";
    }

    public static string Description(
      Pattern dependingNamePattern,
      string dependencyType, 
      Glob dependencyNamePattern)
    {
      return IndependentOf(dependingNamePattern.Description(),
        dependencyType + ":" + dependencyNamePattern.Pattern);
    }
  }
}