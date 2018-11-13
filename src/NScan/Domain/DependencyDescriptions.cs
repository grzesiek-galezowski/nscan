namespace TddXt.NScan.Domain
{
  public static class DependencyDescriptions
  {
    public static string IndependentOf(string dependingAssemblyName, string dependencyAssemblyName)
    {
      return "[" + dependingAssemblyName + "] independentOf [" + dependencyAssemblyName + "]";
    }
  }
}