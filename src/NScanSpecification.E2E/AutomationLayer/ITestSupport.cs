namespace NScanSpecification.E2E.AutomationLayer;

public interface ITestSupport
{
  void RunningDotnetExeWith(string arguments, SolutionDir workingDirectory);
}