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
  extension(Any)
  {
    public static ProjectId ProjectId()
    {
      return Any.Instance<ProjectId>();
    }

    public static Glob Glob()
    {
      return Any.Instance<Glob>();
    }

    public static Pattern Pattern()
    {
      return Any.Instance<Pattern>();
    }

    public static INScanSupport Support()
    {
      return Any.Instance<INScanSupport>();
    }

    public static Seq<T> Seq<T>()
    {
      return Any.List<T>().ToSeq();
    }

    public static Arr<T> Arr<T>()
    {
      return Any.List<T>().ToArr();
    }

    public static HashMap<T, V> HashMap<T, V>() where T : notnull
    {
      return Any.ReadOnlyDictionary<T, V>().ToHashMap();
    }

    public static Map<T, V> Map<T, V>() where T : notnull
    {
      return Any.ReadOnlyDictionary<T, V>().ToMap();
    }

    public static ProjectId ProjectIdOtherThan(ProjectId projectId)
    {
      return Any.OtherThan(projectId);
    }
    
    public static string CSharpFileName()
    {
      return Any.AlphaString() + ".cs";
    }
  }

}
