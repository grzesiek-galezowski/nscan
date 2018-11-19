using System;
using TddXt.NScan.RuleInputData;

namespace TddXt.NScan.Domain
{
  public static class RuleNames
  {
    public const string IndependentOf = "independentOf";
    public const string HasCorrectNamespaces = "hasCorrectNamespaces";

    public static void Switch(RuleDto ruleDto, Action<IndependentRuleComplementDto> independentRuleAction,
      Action<CorrectNamespacesRuleComplementDto> namespacesRuleAction)
    {
      if (ruleDto.RuleName == IndependentOf)
      {
        independentRuleAction(ruleDto.Either.Left);
      }
      else if (ruleDto.RuleName == HasCorrectNamespaces)
      {
        namespacesRuleAction(ruleDto.Either.Right);
      }
      else
      {
        throw new InvalidOperationException($"Unknown rule name {ruleDto.RuleName}");
      }
    }

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