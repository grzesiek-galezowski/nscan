using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AtmaFileSystem;
using AutoFixture;
using Fclp.Internals.Extensions;
using static AtmaFileSystem.AtmaFileSystemPaths;

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
      _projects.AsParallel().ForEach(projectName =>
      {
        var absoluteDirectoryPath = solutionDir.PathToProject(projectName);
        CreateProject(dotNetExe, projectName, absoluteDirectoryPath);
      });
    }

    private static void CreateProject(DotNetExe dotNetExe, string projectName, AbsoluteDirectoryPath projectDirPath)
    {
      ProcessAssertions.AssertSuccess(
        dotNetExe.RunWith($"new classlib --name {projectName}")
          .Result);
      RemoveDefaultFileCreatedByTemplate(projectDirPath);
    }

    private static void RemoveDefaultFileCreatedByTemplate(AbsoluteDirectoryPath projectDirPath)
    {
      File.Delete((projectDirPath + FileName("Class1.cs")).ToString());
    }

  }
}