using GlobExpressions;
using NScan.Lib;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using TddXt.AnyExtensibility;
using TddXt.AnyRoot;

namespace NScanSpecification.Lib
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