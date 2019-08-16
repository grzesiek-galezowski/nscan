using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NScan.Adapter.ReadingCSharpSolution.ReadingProjects;
using NScan.Adapter.ReadingRules;
using NScan.Domain.Root;
using NScan.Lib;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScan.SharedKernel.RuleDtos;
using NScan.SharedKernel.WritingProgramOutput.Ports;
using Sprache;

namespace TddXt.NScan
{
  public static class NScanMain
  {
    /// <summary>
    /// Entry point
    /// </summary>
    /// <param name="inputArguments">arguments</param>
    /// <param name="output">output for report</param>
    /// <param name="support">logging stuff</param>
    /// <returns></returns>
    public static int Run(
      InputArgumentsDto inputArguments, 
      INScanOutput output, 
      INScanSupport support)
    {
      try
      {
        //SpinWait.SpinUntil(() => Debugger.IsAttached);

        output.WriteVersion(Versioning.VersionOf(Assembly.GetExecutingAssembly()));

        var ruleDtos = ReadRules(inputArguments);
        LogRules(ruleDtos, support);
        var xmlProjects = ReadXmlProjects(inputArguments, support);
        var analysis = Analysis.PrepareFor(xmlProjects, support);

        analysis.AddRules(ruleDtos);

        analysis.Run();
        output.WriteAnalysisReport(analysis.Report);
        return analysis.ReturnCode;
      }
      catch (Exception e)
      {
        support.Report(e);
        return -2;
      }
    }

    private static IEnumerable<RuleUnionDto> ReadRules(InputArgumentsDto inputArguments)
    {
      var rulesString = File.ReadAllText(inputArguments.RulesFilePath.ToString());
      var ruleDtos = ParseRule.FromLine().Many().Parse(rulesString);
      return ruleDtos;
    }

    private static void LogRules(
      IEnumerable<RuleUnionDto> enumerable,
      INScanSupport support)
    {
      foreach (var ruleUnion in enumerable)
      {
        ruleUnion.Accept(new RuleLoggingVisitor(support));
      }
    }

    private static List<XmlProject> ReadXmlProjects(InputArgumentsDto cliOptions, INScanSupport support)
    {
      var paths = ProjectPaths.From(
        cliOptions.SolutionPath.ToString(),
        support);
      var xmlProjects = paths.LoadXmlProjects();
      return xmlProjects;
    }
  }
}