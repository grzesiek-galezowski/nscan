using System;
using Xunit;

namespace TddXt.NScan.Specification.ReadingRules.Adapters
{
  public static class AssertionLambdas
  {
    public static Action<T> FailWhen<T>()
    {
      return _ => { Assert.False(true); };
    }
  }
}