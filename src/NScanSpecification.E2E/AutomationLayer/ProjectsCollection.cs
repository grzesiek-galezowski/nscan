using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AtmaFileSystem;
using Microsoft.Build.Utilities.ProjectCreation;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScanSpecification.E2E.AutomationLayer;

public class ProjectsCollection(DotNetExe dotNetExe)
{
  private readonly Dictionary<string, ProjectCreator> _projects = new();

  public void Add(ProjectDefinition projectDefinition)
  {
    _projects[projectDefinition.ProjectName] =
      ProjectCreator.Templates.SdkCsproj(
        targetFramework: projectDefinition.TargetFramework,
        outputType: Any.String()
      );
  }

  public async Task AddToSolution(string solutionName, CancellationToken cancellationToken)
  {
    await dotNetExe.RunWith(
      $"sln {solutionName}.sln add {string.Join(" ", _projects.Select(p => p.Key))}", cancellationToken);
  }

  public Task SaveIn(SolutionDir solutionDir, CancellationToken cancellationToken)
  {
    return Task.WhenAll(_projects.Select(p =>
    {
      var absoluteDirectoryPath = solutionDir.PathToProject(p.Key);
      return CreateProject(p.Key, absoluteDirectoryPath, p.Value, cancellationToken);
    }));
  }

  private async Task CreateProject(
    string projectName, AbsoluteDirectoryPath projectDirPath,
    ProjectCreator projectCreator, 
    CancellationToken cancellationToken)
  {
    //bug add logging
    await Task.Run(() =>
    {
      projectCreator.Save(projectDirPath.AddFileName(projectName + ".csproj").ToString());
      RemoveDefaultFileCreatedByTemplate(projectDirPath);
    }, cancellationToken);
  }

  private static void RemoveDefaultFileCreatedByTemplate(AbsoluteDirectoryPath projectDirPath)
  {
    File.Delete((projectDirPath + FileName("Class1.cs")).ToString());
  }

  public void AddProjectReference(string projectName, string referenceName)
  {
    _projects[projectName].ItemInclude("ProjectReference", $"..\\{referenceName}\\{referenceName}.csproj");
  }
}
