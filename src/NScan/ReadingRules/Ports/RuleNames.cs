using System;

namespace TddXt.NScan.ReadingRules.Ports
{
  public static class RuleNames
  {
    public const string IndependentOf = "independentOf";
    public const string HasCorrectNamespaces = "hasCorrectNamespaces";
    public const string HasNoCircularUsings = "hasNoCircularUsings";
    public const string HasAttributesOn = "hasAttributesOn";
    public const string HasTargetFramework = "hasTargetFramework";

    public static T Switch<T>(string ruleName,
      Func<T> independentOfValueFactory,
      Func<T> correctNamespacesValueFactory, 
      Func<T> noCircularUsingsValueFactory, 
      Func<T> isDecoratedWithAttributeValueFactory,
      Func<T> hasTargetFrameworkValueFactory)
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
      else if(ruleName == HasAttributesOn)
      {
        return isDecoratedWithAttributeValueFactory();
      }
      else if(ruleName == HasTargetFramework)
      {
        return hasTargetFrameworkValueFactory();
      }
      else
      {
        throw new InvalidOperationException($"Unknown rule name {ruleName}");
      }
    }
  }
}