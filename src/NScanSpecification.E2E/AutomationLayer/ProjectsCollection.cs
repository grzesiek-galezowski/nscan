using System.Collections.Generic;
using System.IO;
using System.Linq;
using AtmaFileSystem;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScanSpecification.E2E.AutomationLayer
{
  public class ProjectsCollection
  {
    private readonly DotNetExe _dotNetExe;
    private readonly List<ProjectDefinition> _projects = new List<ProjectDefinition>();

    public ProjectsCollection(DotNetExe dotNetExe)
    {
      _dotNetExe = dotNetExe;
    }

    public void Add(ProjectDefinition projectDefinition)
    {
      _projects.Add(projectDefinition);
    }

    public void AddToSolution(string solutionName)
    {
      ProcessAssertions.AssertSuccess(
        _dotNetExe.RunWith($"sln {solutionName}.sln add {string.Join(" ", _projects.Select(p => p.ProjectName))}")
          .Result);
    }

    public void CreateOnDisk(SolutionDir solutionDir, DotNetExe dotNetExe)
    {
      _projects.ForEach(projectDefinition =>
      {
        var absoluteDirectoryPath = solutionDir.PathToProject(projectDefinition.ProjectName);
        CreateProject(dotNetExe, projectDefinition.ProjectName, absoluteDirectoryPath, projectDefinition.TargetFramework);
      });
    }

    private static void CreateProject(DotNetExe dotNetExe, string projectName, AbsoluteDirectoryPath projectDirPath,
      string targetFramework)
    {
      ProcessAssertions.AssertSuccess(
        dotNetExe.RunWith($"new classlib --name {projectName} -f {targetFramework}")
          .Result);
      RemoveDefaultFileCreatedByTemplate(projectDirPath);
    }

    private static void RemoveDefaultFileCreatedByTemplate(AbsoluteDirectoryPath projectDirPath)
    {
      File.Delete((projectDirPath + FileName("Class1.cs")).ToString());
    }

  }
}