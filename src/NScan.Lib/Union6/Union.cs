using System;

namespace NScan.Lib.Union6;

public abstract class Union<T1, T2, T3, T4, T5, T6>
{
  private readonly object? _value;

  protected Union(T1 o)
  {
    ArgumentNullException.ThrowIfNull(o);
    _value = o;
  }
  protected Union(T2 o)
  {
    ArgumentNullException.ThrowIfNull(o);
    _value = o;
  }
  protected Union(T3 o)
  {
    ArgumentNullException.ThrowIfNull(o);
    _value = o;
  }

  protected Union(T4 o)
  {
    ArgumentNullException.ThrowIfNull(o);
    _value = o;
  }

  protected Union(T5 o)
  {
    ArgumentNullException.ThrowIfNull(o);
    _value = o;
  }
    
  protected Union(T6 o)
  {
    ArgumentNullException.ThrowIfNull(o);
    _value = o;
  }

  public void Accept(IUnionVisitor<T1, T2, T3, T4, T5, T6> visitor)
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
      case T6 o:
        visitor.Visit(o);
        break;
      default:
        throw new InvalidOperationException($"Unknown rule name {_value}");
    }
  }

  public TReturn Accept<TReturn>(IUnionTransformingVisitor<T1, T2, T3, T4, T5, T6, TReturn> transformingVisitor)
  {
    return _value switch
    {
      T1 o => transformingVisitor.Visit(o),
      T2 o => transformingVisitor.Visit(o),
      T3 o => transformingVisitor.Visit(o),
      T4 o => transformingVisitor.Visit(o),
      T5 o => transformingVisitor.Visit(o),
      T6 o => transformingVisitor.Visit(o),
      _ => throw new InvalidOperationException($"Unknown rule name {_value}")
    };
  }
}
