using System;

namespace NScan.Lib
{
  public abstract class Union5<T1, T2, T3, T4, T5>
  {
    private readonly object? _value = null;

    protected Union5(T1 o)
    {
      AssertNotNull(o!);
      _value = o;
    }
    protected Union5(T2 o)
    {
      AssertNotNull(o!);
      _value = o;
    }
    protected Union5(T3 o)
    {
      AssertNotNull(o!);
      _value = o;
    }

    protected Union5(T4 o)
    {
      AssertNotNull(o!);
      _value = o;
    }

    protected Union5(T5 o)
    {
      AssertNotNull(o!);
      _value = o;
    }

    private static void AssertNotNull(object o)
    {
      if (o == null)
      {
        throw new ArgumentNullException(nameof(o));
      }
    }

    public void Accept(IUnion5Visitor<T1, T2, T3, T4, T5> visitor)
    {
      Match(
        visitor.Visit,
        visitor.Visit,
        visitor.Visit,
        visitor.Visit,
        visitor.Visit);
    }

    public TReturn Accept<TReturn>(IUnion5TransformingVisitor<T1, T2, T3, T4, T5, TReturn> transformingVisitor)
    {
      return Match(
        transformingVisitor.Visit,
        transformingVisitor.Visit,
        transformingVisitor.Visit,
        transformingVisitor.Visit,
        transformingVisitor.Visit
      );
    }

    private void Match(
      Action<T1> action1,
      Action<T2> action2,
      Action<T3> action3, 
      Action<T4> action4,
      Action<T5> action5)
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
          action4(o);
          break;
        case T5 o:
          action5(o);
          break;
        default:
          throw new InvalidOperationException($"Unknown rule name {_value}");
      }
    }

    private T Match<T>(
      Func<T1, T> map1,
      Func<T2, T> map2,
      Func<T3, T> map3, 
      Func<T4, T> map4, 
      Func<T5, T> map5)
    {
      return _value switch
      {
        T1 o => map1(o),
        T2 o => map2(o),
        T3 o => map3(o),
        T4 o => map4(o),
        T5 o => map5(o),
        _ => throw new InvalidOperationException($"Unknown rule name {_value}")
      };
    }
  }
}