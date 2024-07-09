namespace NScan.Adapters.SecondarySpecification.ReadingRules;

public static class AssertionLambdas
{
  public static Action<T> FailWhen<T>()
  {
    return _ => { true.Should().BeFalse(); };
  }
}
