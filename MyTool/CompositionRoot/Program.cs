using System;
using System.Linq;
using Buildalyzer;
using MyTool.App;
using Sprache;
using static System.Environment;

namespace MyTool.CompositionRoot
{
  public static class Program
  {
    public static int Main(string[] args)
    {
      string rulesString = $"TddXt.Any* independentOf *Common*{NewLine}";
      var ruleDtos = SingleLine().Many().Parse(rulesString);

      var consoleSupport = new ConsoleSupport();
      var paths = ProjectPaths.From(
        @"C:\Users\ftw637\Documents\GitHub\any\src\netstandard2.0\Any.sln", 
        consoleSupport);
      var xmlProjects = paths.LoadXmlProjects();
      var analysis = Analysis.PrepareFor(xmlProjects, consoleSupport);
      foreach (var ruleDto in ruleDtos)
      {
        analysis.IndependentOfProject(
          ruleDto.DependingPattern, 
          ruleDto.DependencyPattern);
      }
      analysis.Run();
      Console.WriteLine(analysis.Report);
      return analysis.ReturnCode;
    }

    public static Parser<RuleDto> SingleLine()
    {
      return from depending in Parse.AnyChar.Until(Parse.WhiteSpace).Text()
        from ruleName in Parse.AnyChar.Until(Parse.WhiteSpace).Text()
        from dependency in Parse.AnyChar.Until(Parse.LineEnd).Text()
        select new RuleDto
        {
          DependingPattern = depending,
          RuleName = ruleName,
          DependencyPattern = dependency,
        };
    }
  }
}
