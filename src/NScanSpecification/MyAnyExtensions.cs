using TddXt.AnyExtensibility;
using TddXt.AnyRoot;
using TddXt.NScan.App;

namespace TddXt.NScan.Specification
{
  public static class MyAnyExtensions
  {
    public static ProjectId ProjectId(this BasicGenerator gen)
    {
      return gen.Instance<ProjectId>();
    }

    public static ISupport Support(this BasicGenerator gen)
    {
      return gen.Instance<ISupport>();
    }

    public static ProjectId ProjectIdOtherThan(this BasicGenerator gen, ProjectId projectId)
    {
      return gen.OtherThan(projectId);
    }
  }
}