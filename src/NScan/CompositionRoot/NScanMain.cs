using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using GlobExpressions;
using Sprache;
using TddXt.NScan.App;
using TddXt.NScan.Domain;
using TddXt.NScan.RuleInputData;
using TddXt.NScan.Xml;

namespace TddXt.NScan.CompositionRoot
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
      var rulesString = File.ReadAllText(inputArguments.RulesFilePath);
      var ruleDtos = ParseRule.FromLine().Many().Parse(rulesString);
      return ruleDtos;
    }

    private static void LogRules(
      IEnumerable<RuleUnionDto> enumerable,
      INScanSupport support)
    {
      foreach (var either in enumerable)
      {
        either.Switch( 
          independent => support.LogIndependentRule(either.Left), 
          namespaces => support.LogNamespacesRule(either.Right));
      }
    }

    private static List<XmlProject> ReadXmlProjects(InputArgumentsDto cliOptions, INScanSupport support)
    {
      var paths = ProjectPaths.From(
        cliOptions.SolutionPath,
        support);
      var xmlProjects = paths.LoadXmlProjects();
      return xmlProjects;
    }
  }
}