using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AtmaFileSystem;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScanSpecification.E2E.AutomationLayer;

public class ProjectsCollection
{
  private readonly DotNetExe _dotNetExe;
  private readonly List<ProjectDefinition> _projects = new();

  public ProjectsCollection(DotNetExe dotNetExe)
  {
    _dotNetExe = dotNetExe;
  }

  public void Add(ProjectDefinition projectDefinition)
  {
    _projects.Add(projectDefinition);
  }

  public async Task AddToSolutionAsync(string solutionName)
  {
    await _dotNetExe.RunWith(
      $"sln {solutionName}.sln add {string.Join(" ", _projects.Select(p => p.ProjectName))}");
  }

  public Task CreateOnDisk(SolutionDir solutionDir, DotNetExe dotNetExe)
  {
    return Task.WhenAll(_projects.Select(p =>
    {
      var absoluteDirectoryPath = solutionDir.PathToProject(p.ProjectName);
      return CreateProject(dotNetExe, p.ProjectName, absoluteDirectoryPath, p.TargetFramework);
    }));
  }

  private static async Task CreateProject(DotNetExe dotNetExe, string projectName, AbsoluteDirectoryPath projectDirPath,
    string targetFramework)
  {
    await dotNetExe.RunWith($"new classlib --name {projectName} -f {targetFramework} --no-restore");
    RemoveDefaultFileCreatedByTemplate(projectDirPath);
  }

  private static void RemoveDefaultFileCreatedByTemplate(AbsoluteDirectoryPath projectDirPath)
  {
    File.Delete((projectDirPath + FileName("Class1.cs")).ToString());
  }

}