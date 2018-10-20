using System;

namespace TddXt.NScan.Specification
{
  public class A
  {
    public static Action CallTo(Action a)
    {
      return a;
    }
  }
}