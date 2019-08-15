using AtmaFileSystem;
using Fclp;
using NScan.Adapter.NotifyingSupport;
using NScan.SharedKernel.SharedKernel;
using TddXt.NScan.Domain.Root;
using TddXt.NScan.WritingProgramOutput.Adapters;

namespace TddXt.NScan.Console
{
  public static class Program
  {
    public static int Main(string[] args)
    {
      var cliOptions = new InputArgumentsDto();
      var parser = CreateCliParser(cliOptions);
      var commandLineParserResult = parser.Parse(args);
      if (!commandLineParserResult.HasErrors)
      {
        return NScanMain.Run(cliOptions, new ConsoleOutput(), new ConsoleSupport());
      }
      else
      {
        System.Console.Error.WriteLine(commandLineParserResult.ErrorText);
        parser.HelpOption.ShowHelp(parser.Options);
        return 1;
      }
    }

    private static FluentCommandLineParser CreateCliParser(InputArgumentsDto inputArguments)
    {
      var p = new FluentCommandLineParser();

      p.Setup<string>('p', "solution-path")
        .WithDescription("Path to solution file")
        .Callback(path => inputArguments.SolutionPath = AnyFilePath.Value(path))
        .Required();

      p.Setup<string>('r', "rules-file-path")
        .WithDescription("Path to rules file")
        .Callback(path => inputArguments.RulesFilePath = AnyFilePath.Value(path))
        .Required();

      p.SetupHelp("?", "help")
        .Callback(text => System.Console.WriteLine(text));
      return p;
    }
  }
}
