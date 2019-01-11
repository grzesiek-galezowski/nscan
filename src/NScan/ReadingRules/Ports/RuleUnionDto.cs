using System;

namespace TddXt.NScan.ReadingRules.Ports
{
  public class RuleUnionDto
  {
    public IndependentRuleComplementDto IndependentRule { get; private set; }
    public CorrectNamespacesRuleComplementDto CorrectNamespacesRule { get; private set; }
    public NoCircularUsingsRuleComplementDto NoCircularUsingsRule { get; set; }

    public string RuleName { get; private set; }

    public static RuleUnionDto With(
      CorrectNamespacesRuleComplementDto correctNamespacesRuleComplementDto)
    {
      if (correctNamespacesRuleComplementDto == null)
      {
        throw new ArgumentNullException(nameof(correctNamespacesRuleComplementDto));
      }

      return new RuleUnionDto()
      {
        CorrectNamespacesRule = correctNamespacesRuleComplementDto,
        RuleName = correctNamespacesRuleComplementDto.RuleName
      };

    }

    public static RuleUnionDto With(NoCircularUsingsRuleComplementDto noCircularUsingsRuleComplementDto)
    {
      if (noCircularUsingsRuleComplementDto == null)
      {
        throw new ArgumentNullException(nameof(noCircularUsingsRuleComplementDto));
      }

      return new RuleUnionDto()
      {
        NoCircularUsingsRule = noCircularUsingsRuleComplementDto,
        RuleName = noCircularUsingsRuleComplementDto.RuleName
      };

    }

    public static RuleUnionDto With(IndependentRuleComplementDto independentRuleComplementDto)
    {
      if (independentRuleComplementDto == null)
      {
        throw new ArgumentNullException(nameof(independentRuleComplementDto));
      }

      return new RuleUnionDto
      {
        IndependentRule = independentRuleComplementDto,
        RuleName = independentRuleComplementDto.RuleName

      };
    }

    public void Switch(
      Action<IndependentRuleComplementDto> independentRuleAction,
      Action<CorrectNamespacesRuleComplementDto> namespacesRuleAction, 
      Action<NoCircularUsingsRuleComplementDto> noCircularUsingsRuleAction)
    {
      if (RuleName == RuleNames.IndependentOf)
      {
        independentRuleAction(IndependentRule);
      }
      else if (RuleName == RuleNames.HasCorrectNamespaces)
      {
        namespacesRuleAction(CorrectNamespacesRule);
      }
      else if (RuleName == RuleNames.HasNoCircularUsings)
      {
        noCircularUsingsRuleAction(NoCircularUsingsRule);
      }
      else
      {
        throw new InvalidOperationException($"Unknown rule name {RuleName}");
      }
    }

    public T Switch<T>(
      Func<IndependentRuleComplementDto, T> independentRuleMappping,
      Func<CorrectNamespacesRuleComplementDto, T> namespacesRuleMapping, 
      Func<NoCircularUsingsRuleComplementDto, T> noCircularUsingsMapping)
    {
      if (RuleName == RuleNames.IndependentOf)
      {
        return independentRuleMappping(IndependentRule);
      }
      else if (RuleName == RuleNames.HasCorrectNamespaces)
      {
        return namespacesRuleMapping(CorrectNamespacesRule);
      }
      else if (RuleName == RuleNames.HasNoCircularUsings)
      {
        return noCircularUsingsMapping(NoCircularUsingsRule);
      }
      else
      {
        throw new InvalidOperationException($"Unknown rule name {RuleName}");
      }
    }
  }


}