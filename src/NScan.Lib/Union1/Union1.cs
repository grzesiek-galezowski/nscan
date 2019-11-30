using System;

namespace NScan.Lib.Union1
{
  public abstract class Union1<T1>
  {
    private readonly object? _value = null;

    protected Union1(T1 o)
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

    public void Accept(IUnion1Visitor<T1> visitor)
    {
      switch (_value)
      {
        case T1 o:
          visitor.Visit(o);
          break;
        default:
          throw new InvalidOperationException($"Unknown rule name {_value}");
      }
    }

    public TReturn Accept<TReturn>(IUnion1TransformingVisitor<T1, TReturn> transformingVisitor)
    {
      return _value switch
      {
        T1 o => transformingVisitor.Visit(o),
        _ => throw new InvalidOperationException($"Unknown rule name {_value}")
      };
    }
  }
}