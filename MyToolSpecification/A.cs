using System;

namespace MyTool
{
  public class A
  {
    public static Action CallTo(Action a)
    {
      return a;
    }
  }
}