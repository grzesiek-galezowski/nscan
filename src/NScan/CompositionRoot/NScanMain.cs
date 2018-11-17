using System.Collections.Generic;
using System.IO;
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
      var ruleDtos = ReadRules(inputArguments);
      LogRules(ruleDtos, support);
      var xmlProjects = ReadXmlProjects(inputArguments, support);
      var analysis = Analysis.PrepareFor(xmlProjects, support);

      analysis.AddRules(ruleDtos);

      analysis.Run();
      output.WriteAnalysisReport(analysis.Report);
      return analysis.ReturnCode;
    }

    private static void LogRules(IEnumerable<RuleDto> ruleDtos, INScanSupport support)
    {
      foreach (var ruleDto in ruleDtos)
      {
        RuleNames.Switch(ruleDto, 
          independent => support.LogIndependentRule(ruleDto), 
          namespaces => support.LogNamespacesRule(ruleDto));
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

    private static IEnumerable<RuleDto> ReadRules(InputArgumentsDto cliOptions)
    {
      var rulesString = File.ReadAllText(cliOptions.RulesFilePath);
      var ruleDtos = ParseRule.FromLine().Many().Parse(rulesString);
      return ruleDtos;
    }
  }
}