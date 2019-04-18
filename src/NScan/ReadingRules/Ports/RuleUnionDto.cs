using System;

namespace TddXt.NScan.ReadingRules.Ports
{
  public class RuleUnionDto
  {
    private readonly object _value = null;

    private RuleUnionDto(object dto)
    {
      _value = dto;
    }

    public string RuleName => Switch(
      dto => dto.RuleName,
      dto => dto.RuleName,
      dto => dto.RuleName);

    public static RuleUnionDto With(
      CorrectNamespacesRuleComplementDto correctNamespacesRuleComplementDto)
    {
      if (correctNamespacesRuleComplementDto == null)
      {
        throw new ArgumentNullException(nameof(correctNamespacesRuleComplementDto));
      }

      return new RuleUnionDto(correctNamespacesRuleComplementDto);

    }

    public static RuleUnionDto With(NoCircularUsingsRuleComplementDto noCircularUsingsRuleComplementDto)
    {
      if (noCircularUsingsRuleComplementDto == null)
      {
        throw new ArgumentNullException(nameof(noCircularUsingsRuleComplementDto));
      }

      return new RuleUnionDto(noCircularUsingsRuleComplementDto);

    }

    public static RuleUnionDto With(IndependentRuleComplementDto independentRuleComplementDto)
    {
      if (independentRuleComplementDto == null)
      {
        throw new ArgumentNullException(nameof(independentRuleComplementDto));
      }

      return new RuleUnionDto(independentRuleComplementDto);
    }

    public void Switch(
      Action<IndependentRuleComplementDto> independentRuleAction,
      Action<CorrectNamespacesRuleComplementDto> namespacesRuleAction, 
      Action<NoCircularUsingsRuleComplementDto> noCircularUsingsRuleAction)
    {
      switch (_value)
      {
        case IndependentRuleComplementDto dto:
          independentRuleAction(dto);
          break;
        case CorrectNamespacesRuleComplementDto dto:
          namespacesRuleAction(dto);
          break;
        case NoCircularUsingsRuleComplementDto dto:
          noCircularUsingsRuleAction(dto);
          break;
        default:
          throw new InvalidOperationException($"Unknown rule name {_value}");
      }
    }

    public T Switch<T>(
      Func<IndependentRuleComplementDto, T> independentRuleMappping,
      Func<CorrectNamespacesRuleComplementDto, T> namespacesRuleMapping, 
      Func<NoCircularUsingsRuleComplementDto, T> noCircularUsingsMapping)
    {
      switch (_value)
      {
        case IndependentRuleComplementDto dto:
          return independentRuleMappping(dto);
        case CorrectNamespacesRuleComplementDto dto:
          return namespacesRuleMapping(dto);
        case NoCircularUsingsRuleComplementDto dto:
          return noCircularUsingsMapping(dto);
        default:
          throw new InvalidOperationException($"Unknown rule name {_value}");
      }

    }
  }

  public class RuleUnionDto3 : Union3<
    IndependentRuleComplementDto, 
    CorrectNamespacesRuleComplementDto,
    NoCircularUsingsRuleComplementDto>
  {
    public static RuleUnionDto3 With(
      CorrectNamespacesRuleComplementDto correctNamespacesRuleComplementDto)
    {
      return new RuleUnionDto3(correctNamespacesRuleComplementDto);
    }

    public static RuleUnionDto3 With(NoCircularUsingsRuleComplementDto noCircularUsingsRuleComplementDto)
    {
      return new RuleUnionDto3(noCircularUsingsRuleComplementDto);

    }

    public static RuleUnionDto3 With(IndependentRuleComplementDto independentRuleComplementDto)
    {
      return new RuleUnionDto3(independentRuleComplementDto);
    }

    public string RuleName => Switch(
      dto => dto.RuleName,
      dto => dto.RuleName,
      dto => dto.RuleName);

    private RuleUnionDto3(IndependentRuleComplementDto o) : base(o) {}
    private RuleUnionDto3(CorrectNamespacesRuleComplementDto o) : base(o) {}
    private RuleUnionDto3(NoCircularUsingsRuleComplementDto o) : base(o) {}
  }


  public class Union3<T1, T2, T3>
  {
    private readonly object _value = null;

    protected Union3(T1 o)
    {
      if (o == null)
      {
        throw new ArgumentNullException(nameof(o));
      }

      _value = o;
    }
    protected Union3(T2 o)
    {
      if (o == null)
      {
        throw new ArgumentNullException(nameof(o));
      }

      _value = o;
    }
    protected Union3(T3 o)
    {
      if (o == null)
      {
        throw new ArgumentNullException(nameof(o));
      }
      _value = o;
    }

    public void Switch(
      Action<T1> action1,
      Action<T2> action2,
      Action<T3> action3)
    {
      switch (_value)
      {
        case T1 dto:
          action1(dto);
          break;
        case T2 dto:
          action2(dto);
          break;
        case T3 dto:
          action3(dto);
          break;
        default:
          throw new InvalidOperationException($"Unknown rule name {_value}");
      }
    }

    public T Switch<T>(
      Func<T1, T> map1,
      Func<T2, T> map2,
      Func<T3, T> map3)
    {
      switch (_value)
      {
        case T1 o:
          return map1(o);
        case T2 o:
          return map2(o);
        case T3 o:
          return map3(o);
        default:
          throw new InvalidOperationException($"Unknown rule name {_value}");
      }

    }
  }
}