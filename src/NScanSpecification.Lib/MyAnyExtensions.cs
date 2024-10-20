using GlobExpressions;
using LanguageExt;
using NScan.Lib;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using TddXt.AnyExtensibility;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;

namespace NScanSpecification.Lib;

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

  public static Seq<T> Seq<T>(this BasicGenerator gen)
  {
    return gen.List<T>().ToSeq();
  }

  public static Arr<T> Arr<T>(this BasicGenerator gen)
  {
    return gen.List<T>().ToArr();
  }

  public static HashMap<T, V> HashMap<T, V>(this BasicGenerator gen)
  {
    return gen.ReadOnlyDictionary<T, V>().ToHashMap();
  }

  public static Map<T,V> Map<T,V>(this BasicGenerator gen)
  {
    return gen.ReadOnlyDictionary<T,V>().ToMap();
  }

  public static ProjectId ProjectIdOtherThan(this BasicGenerator gen, ProjectId projectId)
  {
    return gen.OtherThan(projectId);
  }

  public static string CSharpFileName(this BasicGenerator any)
  {
    return any.AlphaString() + ".cs";
  }
}
