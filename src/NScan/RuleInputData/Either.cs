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