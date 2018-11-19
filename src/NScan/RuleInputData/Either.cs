using System;
using TddXt.NScan.Domain;

namespace TddXt.NScan.RuleInputData
{
  public class Either<TLeft, TRight>
  {
    public TLeft Left { get; set; }
    public TRight Right { get; set; }

    public string RuleName { get; set; }

    public void Switch(
      Action<TLeft> independentRuleAction,
      Action<TRight> namespacesRuleAction)
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
      Func<TLeft, T> independentRuleAction,
      Func<TRight, T> namespacesRuleAction)
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

  public static class Either
  {
    public static Either<IndependentRuleComplementDto, CorrectNamespacesRuleComplementDto> FromRight(CorrectNamespacesRuleComplementDto correctNamespacesRuleComplementDto)
    {
      return new Either<IndependentRuleComplementDto, CorrectNamespacesRuleComplementDto>()
      {
        Right = correctNamespacesRuleComplementDto,
        RuleName = correctNamespacesRuleComplementDto.RuleName
      };

    }

    public static Either<IndependentRuleComplementDto, CorrectNamespacesRuleComplementDto> FromLeft(IndependentRuleComplementDto independentRuleComplementDto)
    {
      return new Either<IndependentRuleComplementDto, CorrectNamespacesRuleComplementDto>()
      {
        Left = independentRuleComplementDto,
        RuleName = independentRuleComplementDto.RuleName

      };
    }
  }
}