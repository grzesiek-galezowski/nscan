using System;
using System.Collections.Generic;
using Buildalyzer;
using MyTool.App;

namespace MyTool.CompositionRoot
{
  static class Program
  {
    static void Main(string[] args)
    {
      var analyzerManager = new AnalyzerManager(@"C:\Users\grzes\Documents\GitHub\any\src\netstandard2.0\Any.sln");
      var projectMetadatas = new Dictionary<ProjectId, IDotNetProject>();


      foreach (var analyzerManagerProject in analyzerManager.Projects)
      {
        var projectFilePath = analyzerManagerProject.Value.ProjectFile.Path;
        try
        {
          var projectMetadata = CsharpWorkspaceModel.LoadProjectFrom(projectFilePath);
          projectMetadatas.Add(projectMetadata.Id, projectMetadata);
        }
        catch (InvalidOperationException e)
        {
          Console.WriteLine("Invalid format - skipping " + projectFilePath + " because of " + e);
        }
      }

      ISolution solution = new DotNetStandardSolution(projectMetadatas);
      solution.ResolveAllProjectsReferences();
      solution.Print();
    }
  }
}
