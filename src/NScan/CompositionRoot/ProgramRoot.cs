using System;
using System.IO;
using GlobExpressions;
using Sprache;
using TddXt.NScan.App;

namespace TddXt.NScan.CompositionRoot
{
  public static class ProgramRoot
  {
    //bug bug bug when no specific assembly name is set, the AssemblyName field is null!!!
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
      //TODO move this inside Analysis class and make the factory choose a rule
      foreach (var ruleDto in ruleDtos)
      {
        if (ruleDto.DependencyType == "project")
        {
          analysis.IndependentOfProject(ruleDto.DependingPattern, ruleDto.DependencyPattern);
        }
        else if (ruleDto.DependencyType == "package")
        {
          analysis.IndependentOfPackage(ruleDto.DependingPattern, ruleDto.DependencyPattern);
        }
        else if (ruleDto.DependencyType == "assembly")
        {
          analysis.IndependentOfAssembly(ruleDto.DependingPattern, ruleDto.DependencyPattern);
        }
      }

      analysis.Run();
      Console.WriteLine(analysis.Report);
      return analysis.ReturnCode;
    }

    public static Parser<RuleDto> SingleLine()
    {
      return from depending in Parse.AnyChar.Until(Parse.WhiteSpace).Text()
        from ruleName in Parse.AnyChar.Until(Parse.WhiteSpace).Text()
        from dependencyType in Parse.AnyChar.Until(Parse.Char(':')).Text()
        from dependency in Parse.AnyChar.Until(Parse.LineEnd).Text()
        select new RuleDto
        {
          DependingPattern = new Glob(depending),
          RuleName = ruleName,
          DependencyPattern = new Glob(dependency),
          DependencyType = dependencyType
        };
    }
  }
}