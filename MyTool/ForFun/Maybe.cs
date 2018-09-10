using System;

namespace MyTool
{
  public struct Maybe
  {
    public static Maybe<T> Just<T>(T instance) where T : class
    {
      if (instance == null)
      {
        throw new Exception("Cannot create just instance from null");
      }
      return new Maybe<T>(instance); // extracting FromNullable does not preserve T
    }

    public static Maybe<T> Nothing<T>() where T : class
    {
      return new Maybe<T>(null);
    }
  }

  public struct Maybe<T> where T : class
  {
    private readonly T _instance;

    public Maybe(T instance)
    {
      _instance = instance;
    }

    public T Value()
    {
      if (HasValue)
      {
        throw new Exception("no data");
      }

      return _instance;
    }

    public bool HasValue => _instance == null;
  }

}