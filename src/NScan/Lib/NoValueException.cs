using System;

namespace TddXt.NScan.Lib
{
  public class NoValueException<T> : Exception
  {
    public NoValueException() 
      : base($"This Maybe<{typeof(T)}> was asked for value but it does not contain any value")
    {
    }
  }
}