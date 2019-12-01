using System;

namespace NScan.Lib.Union2
{
  public abstract class Union2<T1, T2>
  {
    private readonly object? _value = null;

    protected Union2(T1 o)
    {
      AssertNotNull(o!);
      _value = o;
    }
    protected Union2(T2 o)
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

    public void Accept(IUnion2Visitor<T1, T2> visitor)
    {
      switch (_value)
      {
        case T1 o:
          visitor.Visit(o);
          break;
        case T2 o:
          visitor.Visit(o);
          break;
        default:
          throw new InvalidOperationException($"Unknown rule name {_value}");
      }
    }

    public TReturn Accept<TReturn>(IUnion2TransformingVisitor<T1, T2, TReturn> transformingVisitor)
    {
      return _value switch
      {
        T1 o => transformingVisitor.Visit(o),
        T2 o => transformingVisitor.Visit(o),
        _ => throw new InvalidOperationException($"Unknown rule name {_value}")
      };
    }
  }
}