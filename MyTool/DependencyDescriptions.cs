using MyTool.App;

public static class DependencyDescriptions
{
  public static string IndependentOf(IReferencedProject dependingProject, IReferencedProject referencedProject)
  {
    return "[" + dependingProject + "] independentOf [" + referencedProject + "]";
  }
}