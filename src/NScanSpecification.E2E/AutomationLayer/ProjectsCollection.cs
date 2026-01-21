using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using AtmaFileSystem;
using Microsoft.Build.Utilities.ProjectCreation;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScanSpecification.E2E.AutomationLayer;

public class ProjectsCollection(DotNetExe dotNetExe, ITestSupport support)
{
  private readonly Dictionary<string, ProjectCreator> _projects = [];

  public void Add(ProjectDefinition projectDefinition)
  {
    _projects[projectDefinition.ProjectName] =
      ProjectCreator.Templates.SdkCsproj(
        targetFramework: projectDefinition.TargetFramework,
        outputType: Any.String()
      );
  }

  public async Task AddToSolution(AbsoluteFilePath solutionPath, CancellationToken cancellationToken)
  {
    var solutionFileName = Path.GetFileName(solutionPath.ToString());
    await dotNetExe.RunWith(
      $"sln {solutionFileName} add {string.Join(" ", _projects.Select(p => p.Key))}", cancellationToken);
  }

  public Task SaveIn(SolutionDir solutionDir, CancellationToken cancellationToken)
  {
    return Task.WhenAll(_projects.Select(async p =>
    {
      var absoluteDirectoryPath = solutionDir.PathToProject(p.Key);
      await CreateProject(p.Key, absoluteDirectoryPath, p.Value, cancellationToken);
    }));
  }

  private async Task CreateProject(
    string projectName, AbsoluteDirectoryPath projectDirPath,
    ProjectCreator projectCreator, 
    CancellationToken cancellationToken)
  {
    await Task.Run(() =>
    {
      var projectPath = ProjectPath(projectDirPath, projectName);
      support.CreatingProject(projectPath);
      projectCreator.Save(projectPath.ToString());
      support.CreatedProject(projectPath);
      DeleteDefaultFileCreatedByTemplate(projectDirPath);
    }, cancellationToken);
  }

  private static AbsoluteFilePath ProjectPath(AbsoluteDirectoryPath projectDirPath, string projectName)
  {
    return projectDirPath.AddFileName(projectName + ".csproj");
  }

  public void AddProjectReference(string projectName, string referenceName)
  {
    _projects[projectName].ItemInclude("ProjectReference", $"..\\{referenceName}\\{referenceName}.csproj");
  }

  private void DeleteDefaultFileCreatedByTemplate(AbsoluteDirectoryPath projectDirPath)
  {
    var redundantGeneratedFile = (projectDirPath + FileName("Class1.cs"));
    support.DeletingFile(redundantGeneratedFile);
    File.Delete(redundantGeneratedFile.ToString());
    support.DeletedFile(redundantGeneratedFile);
  }
}
