using System;
using System.Threading.Tasks;
using AtmaFileSystem;
using Fclp;
using NScan.Adapters.Secondary.NotifyingSupport;
using NScan.Adapters.Secondary.ReportingOfResults;

namespace TddXt.NScan.Console;

public class Program
{
  public Action<object> WriteLine { init; get; } = System.Console.WriteLine;

  public static async Task<int> Main(string[] args)
  {
    return await new Program().ExecuteWithAsync(args);
  }

  public async Task<int> ExecuteWithAsync(string[] args)
  {
    var cliOptions = new InputArgumentsDto();
    var parser = CreateCliParser(cliOptions);
    var commandLineParserResult = parser.Parse(args);
    if (!commandLineParserResult.HasErrors)
    {
      return await NScanMain.RunAsync(
        cliOptions,
        new ConsoleOutput(WriteLine),
        new ConsoleSupport(WriteLine));
    }
    else
    {
      System.Console.Error.WriteLine(commandLineParserResult.ErrorText);
      parser.HelpOption.ShowHelp(parser.Options);
      return 1;
    }
  }

  private FluentCommandLineParser CreateCliParser(InputArgumentsDto inputArguments)
  {
    var p = new FluentCommandLineParser();

    p.Setup<string>('p', "solution-path")
      .WithDescription("Path to solution file (.sln or .slnx)")
      .Callback(path => inputArguments.SolutionPath = AnyFilePath.Value(path))
      .Required();

    p.Setup<string>('r', "rules-file-path")
      .WithDescription("Path to rules file")
      .Callback(path => inputArguments.RulesFilePath = AnyFilePath.Value(path))
      .Required();

    p.SetupHelp("?", "help")
      .Callback(text => WriteLine(text));
    return p;
  }
}
