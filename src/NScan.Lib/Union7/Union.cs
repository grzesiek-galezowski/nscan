using System;

namespace NScan.Lib.Union7;

public abstract class Union<T1, T2, T3, T4, T5, T6, T7>
{
  private readonly object? _value;

  protected Union(T1 o)
  {
    AssertNotNull(o!);
    _value = o;
  }
  protected Union(T2 o)
  {
    AssertNotNull(o!);
    _value = o;
  }
  protected Union(T3 o)
  {
    AssertNotNull(o!);
    _value = o;
  }

  protected Union(T4 o)
  {
    AssertNotNull(o!);
    _value = o;
  }

  protected Union(T5 o)
  {
    AssertNotNull(o!);
    _value = o;
  }
    
  protected Union(T6 o)
  {
    AssertNotNull(o!);
    _value = o;
  }

  protected Union(T7 o)
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

  public void Accept(IUnionVisitor<T1, T2, T3, T4, T5, T6, T7> visitor)
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
      case T7 o:
        visitor.Visit(o);
        break;
      default:
        throw new InvalidOperationException($"Unknown rule name {_value}");
    }
  }

  public TReturn Accept<TReturn>(IUnionTransformingVisitor<T1, T2, T3, T4, T5, T6, T7, TReturn> transformingVisitor)
  {
    return _value switch
    {
      T1 o => transformingVisitor.Visit(o),
      T2 o => transformingVisitor.Visit(o),
      T3 o => transformingVisitor.Visit(o),
      T4 o => transformingVisitor.Visit(o),
      T5 o => transformingVisitor.Visit(o),
      T6 o => transformingVisitor.Visit(o),
      T7 o => transformingVisitor.Visit(o),
      _ => throw new InvalidOperationException($"Unknown rule name {_value}")
    };
  }
}