using System;

namespace NScanSpecification.Lib.AutomationLayer
{
  public static class NullableReferenceTypesExtensions
  {
    public static T OrThrow<T>(this T? instance) where T : class
    {
      return instance.OrThrow(nameof(instance));
    }

    public static T OrThrow<T>(this T? instance, string instanceName) where T : class
    {
      return instance ?? throw new ArgumentNullException(instanceName);
    }

    public static TResult? Select<T, TResult>(this T? a, Func<T, TResult> fn) 
      where T: class 
      where TResult : class
    {
      return a == null ? null : fn(a);
    }

    public static T OrElse<T>(this T? a, Func<T> @default) where T : class
    {
      return a ?? @default();
    }
  }
}