using System;

namespace TddXt.NScan.ReadingRules.Ports
{
  public abstract class Union3<T1, T2, T3>
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

    public void Match(
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

    public T Match<T>(
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