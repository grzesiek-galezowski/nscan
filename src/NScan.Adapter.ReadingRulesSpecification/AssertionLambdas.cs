using System;
using Xunit;

namespace NScan.Adapter.ReadingRulesSpecification
{
  public static class AssertionLambdas
  {
    public static Action<T> FailWhen<T>()
    {
      return _ => { Assert.False(true); };
    }
  }
}
