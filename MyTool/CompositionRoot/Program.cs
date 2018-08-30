using System;
using System.Collections.Generic;
using System.Linq;
using Buildalyzer;
using MyTool.App;

namespace MyTool.CompositionRoot
{
  static class Program
  {
    public static void Main(string[] args)
    {
      var analyzerManager = new AnalyzerManager(@"C:\Users\grzes\Documents\GitHub\any\src\netstandard2.0\Any.sln");
      var projectFilePaths = analyzerManager.Projects.Select(p => p.Value.ProjectFile.Path).ToList();

      var projects = CsharpWorkspaceModel.LoadProjectsPointedToBy(projectFilePaths);
      var solution = new DotNetStandardSolution(projects);
      solution.ResolveAllProjectsReferences();
      solution.Print();
    }
  }
}
