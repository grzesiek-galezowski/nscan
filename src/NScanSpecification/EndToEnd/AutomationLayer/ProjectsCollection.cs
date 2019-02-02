using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public class ProjectsCollection
  {
    private readonly DotNetExe _dotNetExe;
    private readonly List<string> _projects = new List<string>();

    public ProjectsCollection(DotNetExe dotNetExe)
    {
      _dotNetExe = dotNetExe;
    }

    public void Add(string projectName)
    {
      _projects.Add(projectName);
    }

    public void AddToSolution(string solutionName)
    {
      ProcessAssertions.AssertSuccess(
        _dotNetExe.RunWith($"sln {solutionName}.sln add {string.Join(" ", _projects)}")
          .Result);
    }

    public void CreateOnDisk(SolutionDir solutionDir, DotNetExe dotNetExe)
    {
      _projects.AsParallel().ForAll(projectName => CreateProjectAsync(dotNetExe, projectName, 
        solutionDir.PathToProject(projectName)));
    }

    private static void CreateProjectAsync(DotNetExe dotNetExe, string projectName, string projectDirPath)
    {
      ProcessAssertions.AssertSuccess(
        dotNetExe.RunWith($"new classlib --name {projectName}")
          .Result);
      RemoveDefaultFileCreatedbyTemplate(projectDirPath);
    }

    private static void RemoveDefaultFileCreatedbyTemplate(string projectDirPath)
    {
      File.Delete(Path.Combine(projectDirPath, "Class1.cs"));
    }

  }
}