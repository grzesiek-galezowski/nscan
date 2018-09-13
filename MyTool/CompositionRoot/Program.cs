using System;
using System.Linq;
using Buildalyzer;
using MyTool.App;

namespace MyTool.CompositionRoot
{
  internal static class Program
  {
    public static int Main(string[] args)
    {
      var analyzerManager = new AnalyzerManager(@"C:\Users\ftw637\Documents\GitHub\any\src\netstandard2.0\Any.sln");
      var projectFilePaths = analyzerManager.Projects.Select(p => p.Value.ProjectFile.Path).ToList();
      var consoleSupport = new ConsoleSupport();
      var paths = new ProjectPaths(projectFilePaths, consoleSupport);
      var xmlProjects = paths.LoadXmlProjects();

      var analysis = Analysis.PrepareFor(xmlProjects, consoleSupport);
      analysis.IndependentOfProject("TddXt.Any*", "*Common*");
      analysis.Run();
      Console.WriteLine(analysis.Report);
      return analysis.ReturnCode;
    }
  }
}
