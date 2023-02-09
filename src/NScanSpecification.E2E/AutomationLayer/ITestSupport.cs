namespace NScanSpecification.E2E.AutomationLayer;

public interface ITestSupport
{
  void RunningDotnetExeWith(string arguments, SolutionDir workingDirectory);
  void DotnetExeFinished(int exitCode, string standardOutput, string standardError);
}
