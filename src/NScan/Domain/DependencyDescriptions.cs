using GlobExpressions;

namespace TddXt.NScan.Domain
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
      //TODO consider moving to DependencyDescriptions
      return DependencyDescriptions.IndependentOf(dependingNamePattern.Description(),
        dependencyType + ":" + dependencyNamePattern.Pattern);
    }
  }
}