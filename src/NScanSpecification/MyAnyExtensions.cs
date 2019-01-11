using TddXt.AnyExtensibility;
using TddXt.AnyRoot;
using TddXt.NScan.Domain;
using TddXt.NScan.NotifyingSupport.Ports;

namespace TddXt.NScan.Specification
{
  public static class MyAnyExtensions
  {
    public static ProjectId ProjectId(this BasicGenerator gen)
    {
      return gen.Instance<ProjectId>();
    }

    public static INScanSupport Support(this BasicGenerator gen)
    {
      return gen.Instance<INScanSupport>();
    }

    public static ProjectId ProjectIdOtherThan(this BasicGenerator gen, ProjectId projectId)
    {
      return gen.OtherThan(projectId);
    }
  }
}