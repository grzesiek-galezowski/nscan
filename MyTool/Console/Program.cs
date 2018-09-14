﻿using System;
using Fclp;
using Root;

namespace MyTool.CompositionRoot
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
        return ProgramRoot.RunProgram(cliOptions);
      }
      else
      {
        Console.Error.WriteLine(commandLineParserResult.ErrorText);
        parser.HelpOption.ShowHelp(parser.Options);
        return 1;
      }
    }


    private static FluentCommandLineParser CreateCliParser(InputArgumentsDto inputArguments)
    {
      var p = new FluentCommandLineParser();

      p.Setup<string>('p', "solution-path")
        .WithDescription("Path to solution file")
        .Callback(path => inputArguments.SolutionPath = path)
        .Required();

      p.Setup<string>('r', "rules-file-path")
        .WithDescription("Path to rules file")
        .Callback(path => inputArguments.RulesFilePath = path)
        .Required();

      p.SetupHelp("?", "help")
        .Callback(text => Console.WriteLine(text));
      return p;
    }
  }
}
