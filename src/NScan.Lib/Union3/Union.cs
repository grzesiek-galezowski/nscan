﻿using System;

namespace NScan.Lib.Union3;

public abstract class Union<T1, T2, T3>
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

  private static void AssertNotNull(object o)
  {
    if (o == null)
    {
      throw new ArgumentNullException(nameof(o));
    }
  }

  public void Accept(IUnionVisitor<T1, T2, T3> visitor)
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
      default:
        throw new InvalidOperationException($"Unknown rule name {_value}");
    }
  }

  public TReturn Accept<TReturn>(IUnionTransformingVisitor<T1, T2, T3, TReturn> transformingVisitor)
  {
    return _value switch
    {
      T1 o => transformingVisitor.Visit(o),
      T2 o => transformingVisitor.Visit(o),
      T3 o => transformingVisitor.Visit(o),
      _ => throw new InvalidOperationException($"Unknown rule name {_value}")
    };
  }
}