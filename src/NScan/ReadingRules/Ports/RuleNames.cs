using System;

namespace TddXt.NScan.ReadingRules.Ports
{
  public static class RuleNames
  {
    public const string IndependentOf = "independentOf";
    public const string HasCorrectNamespaces = "hasCorrectNamespaces";
    public const string HasNoCircularUsings = "hasNoCircularUsings";
    public const string HasAnnotationsOn = "hasAnnotationsOn";

    public static T Switch<T>(string ruleName,
      Func<T> independentOfValueFactory,
      Func<T> correctNamespacesValueFactory, 
      Func<T> noCircularUsingsValueFactory, 
      Func<T> isAnnotatedValueFactory)
    {
      if (ruleName == IndependentOf)
      {
        return independentOfValueFactory();
      }
      else if (ruleName == HasCorrectNamespaces)
      {
        return correctNamespacesValueFactory();
      }
      else if(ruleName == HasNoCircularUsings)
      {
        return noCircularUsingsValueFactory();
      }
      else if(ruleName == HasAnnotationsOn)
      {
        return isAnnotatedValueFactory();
      }
      else
      {
        throw new InvalidOperationException($"Unknown rule name {ruleName}");
      }
    }
  }
}