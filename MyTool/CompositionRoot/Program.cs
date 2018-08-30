using System.Linq;
using Buildalyzer;

namespace MyTool.CompositionRoot
{
  static class Program
  {
    public static void Main(string[] args)
    {
      var analyzerManager = new AnalyzerManager(@"C:\Users\grzes\Documents\GitHub\any\src\netstandard2.0\Any.sln");
      var projectFilePaths = analyzerManager.Projects.Select(p => p.Value.ProjectFile.Path).ToList();
      var projects = CsharpWorkspaceModel.LoadProjectsPointedToBy(projectFilePaths);

      Analysis.Of(projects).Run();
    }
  }
}
