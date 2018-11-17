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
        independentRuleAction(ruleDto.IndependentRuleComplement);
      }
      else if (ruleDto.RuleName == HasCorrectNamespaces)
      {
        namespacesRuleAction(ruleDto.CorrectNamespacesRuleComplement);
      }
      else
      {
        throw new InvalidOperationException($"Unknown rule name {ruleDto.RuleName}");
      }
    }
  }
}