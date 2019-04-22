using System;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.ReadingSolution.Lib
{
  public abstract class Union4<T1, T2, T3, T4>
  {
    private readonly object _value = null;

    protected Union4(T1 o)
    {
      if (o == null)
      {
        throw new ArgumentNullException(nameof(o));
      }

      _value = o;
    }
    protected Union4(T2 o)
    {
      if (o == null)
      {
        throw new ArgumentNullException(nameof(o));
      }

      _value = o;
    }
    protected Union4(T3 o)
    {
      if (o == null)
      {
        throw new ArgumentNullException(nameof(o));
      }
      _value = o;
    }

    protected Union4(T4 o)
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
        case T1 o:
          action1(o);
          break;
        case T2 o:
          action2(o);
          break;
        case T3 o:
          action3(o);
          break;
        case T4 o:
          new Action<T4>(o1 => { })(o);
          break;
        default:
          throw new InvalidOperationException($"Unknown rule name {_value}");
      }
    }

    public T Match<T>(
      Func<T1, T> map1,
      Func<T2, T> map2,
      Func<T3, T> map3, 
      Func<T4, T> map4)
    {
      switch (_value)
      {
        case T1 o:
          return map1(o);
        case T2 o:
          return map2(o);
        case T3 o:
          return map3(o);
        case T4 o:
          return map4(o);
        default:
          throw new InvalidOperationException($"Unknown rule name {_value}");
      }

    }
  }
}