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
      switch (_value)
      {
        case T1 o:
          visitor.Visit(o);
          break;
        case T2 o:
          visitor.Visit(o);
          break;
        case T3 o:
          visitor.Visit(o);
          break;
        case T4 o:
          visitor.Visit(o);
          break;
        case T5 o:
          visitor.Visit(o);
          break;
        default:
          throw new InvalidOperationException($"Unknown rule name {_value}");
      }
    }

    public TReturn Accept<TReturn>(IUnion5TransformingVisitor<T1, T2, T3, T4, T5, TReturn> transformingVisitor)
    {
      return _value switch
      {
        T1 o => transformingVisitor.Visit(o),
        T2 o => transformingVisitor.Visit(o),
        T3 o => transformingVisitor.Visit(o),
        T4 o => transformingVisitor.Visit(o),
        T5 o => transformingVisitor.Visit(o),
        _ => throw new InvalidOperationException($"Unknown rule name {_value}")
      };
    }
  }
}