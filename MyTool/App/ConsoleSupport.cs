using System;

namespace MyTool.App
{
  public interface ISupport
  {
    void Report(ReferencedProjectNotFoundInSolutionException exceptionFromResolution);
  }

  public class ConsoleSupport : ISupport
  {
    public void Report(ReferencedProjectNotFoundInSolutionException exceptionFromResolution)
    {
      Console.WriteLine(exceptionFromResolution);
    }
  }
}