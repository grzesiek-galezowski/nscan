using System;
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
        Console.WriteLine(cliOptions.RulesFilePath);
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
      else
      {
        Console.Error.WriteLine(commandLineParserResult.ErrorText);
        parser.HelpOption.ShowHelp(parser.Options);
        return 1;
      }

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

  internal class CliParsingException : Exception
  {
    public CliParsingException(string errorText) : base(errorText)
    {
      
    }
  }

  public class CliOptionsDto
  {
    public string SolutionPath { get; set; }
    public string RulesFilePath { get; set; }
  }
}
