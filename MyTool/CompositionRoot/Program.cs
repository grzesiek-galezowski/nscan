using System;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using Buildalyzer;
using Fclp;
using MyTool.App;
using Sprache;
using static System.Environment;

namespace MyTool.CompositionRoot
{
  public static class Program
  {
    public static int Main(string[] args)
    {
      var cliOptions = new CliOptionsDto();
      var parser = CreateCliParser(cliOptions);
      var commandLineParserResult = parser.Parse(args);
      if (!commandLineParserResult.HasErrors)
      {
        return RunProgram(cliOptions);
      }
      else
      {
        Console.Error.WriteLine(commandLineParserResult.ErrorText);
        parser.HelpOption.ShowHelp(parser.Options);
        return 1;
      }
    }

    private static int RunProgram(CliOptionsDto cliOptions)
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


    private static FluentCommandLineParser CreateCliParser(CliOptionsDto cliOptions)
    {
      var p = new FluentCommandLineParser();

      p.Setup<string>('p', "solution-path")
        .WithDescription("Path to solution file")
        .Callback(path => cliOptions.SolutionPath = path)
        .Required();

      p.Setup<string>('r', "rules-file-path")
        .WithDescription("Path to rules file")
        .Callback(path => cliOptions.RulesFilePath = path)
        .Required();

      p.SetupHelp("?", "help")
        .Callback(text => Console.WriteLine(text));
      return p;
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

  public class CliOptionsDto
  {
    public string SolutionPath { get; set; }
    public string RulesFilePath { get; set; }
  }
}
