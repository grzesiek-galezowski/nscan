using System;
using System.IO;
using NScanRoot.App;
using Sprache;
using static Sprache.Parse;

namespace NScanRoot.CompositionRoot
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
      return from depending in AnyChar.Until(WhiteSpace).Text()
        from ruleName in AnyChar.Until(WhiteSpace).Text()
        from dependency in AnyChar.Until(LineEnd).Text()
        select new RuleDto
        {
          DependingPattern = depending,
          RuleName = ruleName,
          DependencyPattern = dependency,
        };
    }
  }
}