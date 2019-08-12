using GlobExpressions;
using TddXt.AnyExtensibility;
using TddXt.AnyRoot;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Specification
{
  public static class MyAnyExtensions
  {
    public static ProjectId ProjectId(this BasicGenerator gen)
    {
      return gen.Instance<ProjectId>();
    }

    public static Glob Glob(this BasicGenerator gen)
    {
      return gen.Instance<Glob>();
    }

    public static Pattern Pattern(this BasicGenerator gen)
    {
      return gen.Instance<Pattern>();
    }

    public static INScanSupport Support(this BasicGenerator gen)
    {
      return gen.Instance<INScanSupport>();
    }

    public static ProjectId ProjectIdOtherThan(this BasicGenerator gen, ProjectId projectId)
    {
      return gen.OtherThan(projectId);
    }

    public static string CSharpFileName(this BasicGenerator any)
    {
      return "lol.cs";
    }
  }
}