using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sprache;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingRules.Adapters;
using TddXt.NScan.ReadingRules.Ports;
using TddXt.NScan.ReadingSolution.Adapters;
using TddXt.NScan.ReadingSolution.Ports;
using TddXt.NScan.WritingProgramOutput.Ports;

namespace TddXt.NScan.Domain.Root
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
        ruleUnion.Match( 
          support.Log, 
          support.Log, 
          support.Log,
          support.Log);
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