using System;

namespace TddXt.NScan.Domain
{
  public static class RuleNames
  {
    public const string IndependentOf = "independentOf";
    public const string HasCorrectNamespaces = "hasCorrectNamespaces";

    public static T Switch<T>(string ruleName,
      Func<T> independentOfValueFactory,
      Func<T> correctNamespacesValueFactory)
    {
      if (ruleName == IndependentOf)
      {
        return independentOfValueFactory();
      }
      else if (ruleName == HasCorrectNamespaces)
      {
        return correctNamespacesValueFactory();
      }
      else
      {
        throw new InvalidOperationException($"Unknown rule name {ruleName}");
      }
    }
  }
}