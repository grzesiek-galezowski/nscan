using System;
using System.IO;
using System.Reflection;
using Core.Maybe;
using NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects;
using NScan.Adapters.Secondary.ReadingRules;
using NScan.DependencyPathBasedRules;
using NScan.Lib;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using NScan.SharedKernel.WritingProgramOutput.Ports;
using Core.NullableReferenceTypesExtensions;
using LanguageExt;
using Sprache;
using TddXt.NScan.Domain;

namespace TddXt.NScan;

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

      var csharpProjectDtos = ReadCsharpProjects(inputArguments, support);
      var analysis = Analysis.PrepareFor(csharpProjectDtos, support);

      var rulesString = ReadRulesTextFrom(inputArguments);
        
      var dependencyPathDtos = ParserRulePreface.Then(ParseDependencyPathBasedRule.Complement).Many().Parse(rulesString).WhereValueExist().ToSeq();
      LogDependencyPathRules(dependencyPathDtos, support);
      analysis.AddDependencyPathRules(dependencyPathDtos);
        
      var projectScopedDtos = ParserRulePreface.Then(ParseProjectScopedRule.Complement).Many().Parse(rulesString).WhereValueExist().ToSeq();
      LogProjectScopedRules(projectScopedDtos, support);
      analysis.AddProjectScopedRules(projectScopedDtos);

      var namespaceBasedDtos = ParserRulePreface.Then(ParseNamespaceBasedRule.Complement).Many().Parse(rulesString).WhereValueExist().ToSeq();
      LogNamespaceBasedRules(namespaceBasedDtos, support);
      analysis.AddNamespaceBasedRules(namespaceBasedDtos);

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

  private static Seq<CsharpProjectDto> ReadCsharpProjects(InputArgumentsDto inputArguments, INScanSupport support)
  {
    var msBuildSolution = MsBuildSolution.From(
      inputArguments.SolutionPath.OrThrow(),
      support);
    return msBuildSolution.LoadCsharpProjects();
  }

  private static string ReadRulesTextFrom(InputArgumentsDto inputArguments)
  {
    return File.ReadAllText(inputArguments.RulesFilePath.OrThrow().ToString());
  }

  private static void LogDependencyPathRules(
    Seq<DependencyPathBasedRuleUnionDto> enumerable,
    INScanSupport support)
  {
    foreach (var ruleUnion in enumerable)
    {
      ruleUnion.Accept(new PathBasedRuleLoggingVisitor(support));
    }
  }
  private static void LogProjectScopedRules(
    Seq<ProjectScopedRuleUnionDto> enumerable,
    INScanSupport support)
  {
    foreach (var ruleUnion in enumerable)
    {
      ruleUnion.Accept(new ProjectScopedRuleLoggingVisitor(support));
    }
  }
  private static void LogNamespaceBasedRules(
    Seq<NamespaceBasedRuleUnionDto> enumerable,
    INScanSupport support)
  {
    foreach (var ruleUnion in enumerable)
    {
      ruleUnion.Accept(new NamespaceBasedRuleLoggingVisitor(support));
    }
  }
}
