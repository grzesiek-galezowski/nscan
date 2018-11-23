using System;
using TddXt.NScan.Domain;

namespace TddXt.NScan.RuleInputData
{
  public class RuleUnionDto
  {
    public IndependentRuleComplementDto Left { get; set; }
    public CorrectNamespacesRuleComplementDto Right { get; set; }

    public string RuleName { get; set; }

    public static RuleUnionDto With(CorrectNamespacesRuleComplementDto correctNamespacesRuleComplementDto)
    {
      return new RuleUnionDto()
      {
        Right = correctNamespacesRuleComplementDto,
        RuleName = correctNamespacesRuleComplementDto.RuleName
      };

    }

    public static RuleUnionDto With(IndependentRuleComplementDto independentRuleComplementDto)
    {
      return new RuleUnionDto()
      {
        Left = independentRuleComplementDto,
        RuleName = independentRuleComplementDto.RuleName

      };
    }

    public void Switch(
      Action<IndependentRuleComplementDto> independentRuleAction,
      Action<CorrectNamespacesRuleComplementDto> namespacesRuleAction)
    {
      if (RuleName == RuleNames.IndependentOf)
      {
        independentRuleAction(Left);
      }
      else if (RuleName == RuleNames.HasCorrectNamespaces)
      {
        namespacesRuleAction(Right);
      }
      else
      {
        throw new InvalidOperationException($"Unknown rule name {RuleName}");
      }
    }

    public T Switch<T>(
      Func<IndependentRuleComplementDto, T> independentRuleAction,
      Func<CorrectNamespacesRuleComplementDto, T> namespacesRuleAction)
    {
      if (RuleName == RuleNames.IndependentOf)
      {
        return independentRuleAction(Left);
      }
      else if (RuleName == RuleNames.HasCorrectNamespaces)
      {
        return namespacesRuleAction(Right);
      }
      else
      {
        throw new InvalidOperationException($"Unknown rule name {RuleName}");
      }
    }


    public T Switch<T>(Func<T> whenLeft, Func<T> whenRight)
    {
      if (Left == null)
      {
        return whenLeft();
      }
      else
      {
        return whenRight();
      }
    }
  }


}