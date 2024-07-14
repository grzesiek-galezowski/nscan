using AtmaFileSystem;

namespace NScanSpecification.E2E.AutomationLayer;

public interface ITestSupport
{
  void RunningDotnetExeWith(string arguments, SolutionDir workingDirectory);
  void DotnetExeFinished(int exitCode, string standardOutput, string standardError);
  void DeletingFile(AbsoluteFilePath redundantGeneratedFile);
  void DeletedFile(AbsoluteFilePath redundantGeneratedFile);
  void CreatingProject(AbsoluteFilePath projectPath);
  void CreatedProject(AbsoluteFilePath projectPath);
}
