using System;
using TddXt.NScan.CompositionRoot;

namespace TddXt.NScan.ForFun
{
  public struct Maybe
  {
    public static Maybe<T> Just<T>(T instance)
    {
      if (instance == null)
      {
        throw new ArgumentNullException(nameof(instance));
      }
      return new Maybe<T>(instance);
    }

    public static Maybe<T> Nothing<T>()
    {
      return new Maybe<T>();
    }
  }

  public struct Maybe<T>
  {
    private readonly T _instance;

    public Maybe(T instance)
    {
      HasValue = true;
      _instance = instance;
    }

    public T Value()
    {
      if (!HasValue)
      {
        throw new Exception("no data");
      }

      return _instance;
    }

    public bool HasValue { get; }

    public Maybe<TU> Select<TU>(Func<T, TU> func)
    {
      if (HasValue)
      {
        return Maybe.Just(func(_instance));
      }
      else
      {
        return Maybe.Nothing<TU>();
      }
    }

    public T Otherwise(Func<T> fallbackFunc)
    {
      if (HasValue)
      {
        return Value();
      }
      else
      {
        return fallbackFunc();
      }
    }
  }

}