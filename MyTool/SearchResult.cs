using System;
using System.Collections.Generic;
using System.Linq;
using MyTool.App;

namespace MyTool
{
  internal class SearchResult<T>
  {
    private readonly T _instance;
    private readonly int _index; 

    public SearchResult(T instance, int index)
    {
      _instance = instance;
      _index = index;
    }

    public bool Found  => _instance != null;
    public T Value => Found ? _instance : throw new NoValueException(typeof(T));

    public List<T> SegmentEndingWith(SearchResult<T> second, IEnumerable<T> path) => path.ToList().GetRange(_index, second._index - _index + 1);

    public bool IsBefore(SearchResult<T> dependency)
    {
      return _index < dependency._index;
    }
  }

  internal static class SearchResult
  {
    public static SearchResult<T> ItemFound<T>(T instance, int i)
    {
      return new SearchResult<T>(instance, i);
    }

    public static SearchResult<IReferencedProject> ItemNotFound()
    {
      return new SearchResult<IReferencedProject>(null, 0);
    }
  }

  public class NoValueException : Exception
  {
    public NoValueException(Type type) : base("No value of type " + type + " in search result")
    {

    }
  }
}