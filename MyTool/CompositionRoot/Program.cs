using System.Linq;
using Buildalyzer;
using MyTool.App;

namespace MyTool.CompositionRoot
{
  static class Program
  {
    public static void Main(string[] args)
    {
      var analyzerManager = new AnalyzerManager(@"C:\Users\ftw637\Documents\GitHub\any\src\netstandard2.0\Any.sln");
      var projectFilePaths = analyzerManager.Projects.Select(p => p.Value.ProjectFile.Path).ToList();
      var projects = new CsharpWorkspaceModel(
        new ConsoleSupport())
        .LoadProjectsPointedToBy(projectFilePaths);

      Analysis.Of(projects).Run();
    }
  }
}
