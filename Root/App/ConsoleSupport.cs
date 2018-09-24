using System;

namespace NScanRoot.App
{
  public interface ISupport
  {
    void Report(ReferencedProjectNotFoundInSolutionException exceptionFromResolution);
    void SkippingProjectBecauseOfError(InvalidOperationException invalidOperationException, string projectFilePath);
  }

  public class ConsoleSupport : ISupport
  {
    public void Report(ReferencedProjectNotFoundInSolutionException exceptionFromResolution)
    {
      Console.WriteLine(exceptionFromResolution);
    }

    public void SkippingProjectBecauseOfError(InvalidOperationException invalidOperationException, string projectFilePath)
    {
      Console.WriteLine("Invalid format - skipping " + projectFilePath + " because of " + invalidOperationException);
    }
  }
}