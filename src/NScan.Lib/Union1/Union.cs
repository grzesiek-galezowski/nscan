using System;

namespace NScan.Lib.Union1;

public abstract class Union<T1>
{
  private readonly object? _value;

  protected Union(T1 o)
  {
    ArgumentNullException.ThrowIfNull(o);
    _value = o;
  }

  public void Accept(IUnionVisitor<T1> visitor)
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

  public TReturn Accept<TReturn>(IUnionTransformingVisitor<T1, TReturn> transformingVisitor)
  {
    return _value switch
    {
      T1 o => transformingVisitor.Visit(o),
      _ => throw new InvalidOperationException($"Unknown rule name {_value}")
    };
  }
}
