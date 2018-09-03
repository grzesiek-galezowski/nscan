using System;

namespace MyToolSpecification
{
  public class A
  {
    public static Action CallTo(Action a)
    {
      return a;
    }
  }
}