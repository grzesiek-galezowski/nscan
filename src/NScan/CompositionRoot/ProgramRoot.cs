using System;
using System.IO;
using Sprache;
using TddXt.NScan.App;

namespace TddXt.NScan.CompositionRoot
{
  public static class ProgramRoot
  {
    public static int RunProgramInConsole(InputArgumentsDto cliOptions)
    {
      string rulesString = File.ReadAllText(cliOptions.RulesFilePath);
      var ruleDtos = SingleLine().Many().Parse(rulesString);

      var consoleSupport = new ConsoleSupport();
      var paths = ProjectPaths.From(
        cliOptions.SolutionPath,
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