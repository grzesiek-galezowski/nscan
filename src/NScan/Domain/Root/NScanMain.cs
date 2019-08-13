using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using NScan.SharedKernel.Ports;
using NScan.SharedKernel.SharedKernel;
using Sprache;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingRules.Adapters;
using TddXt.NScan.ReadingSolution.Adapters;
using TddXt.NScan.ReadingSolution.Ports;
using TddXt.NScan.WritingProgramOutput.Ports;

namespace TddXt.NScan.Domain.Root
{
  public class RuleLoggingVisitor : IRuleDtoVisitor
  {
    private readonly INScanSupport _support;

    public RuleLoggingVisitor(INScanSupport support)
    {
      _support = support;
    }

    public void Visit(HasTargetFrameworkRuleComplementDto dto)
    {
      _support.Log(dto);
    }

    public void Visit(HasAttributesOnRuleComplementDto dto)
    {
      _support.Log(dto);
    }

    public void Visit(NoCircularUsingsRuleComplementDto dto)
    {
      _support.Log(dto);
    }

    public void Visit(CorrectNamespacesRuleComplementDto dto)
    {
      _support.Log(dto);
    }

    public void Visit(IndependentRuleComplementDto dto)
    {
      _support.Log(dto);
    }
  }

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