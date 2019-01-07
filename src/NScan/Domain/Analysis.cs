using System;
using System.Collections.Generic;
using System.Linq;
using TddXt.NScan.App;
using TddXt.NScan.CompositionRoot;
using TddXt.NScan.RuleInputData;
using TddXt.NScan.Xml;

namespace TddXt.NScan.Domain
{
  public class Analysis
  {
    public const int ReturnCodeOk = 0;
    public const int ReturnCodeAnalysisFailed = -1;

    private readonly ISolution _solution;
    private readonly IPathRuleSet _pathRules;
    private readonly IProjectScopedRuleSet _projectScopedRules;
    private readonly IAnalysisReportInProgress _analysisReportInProgress;
    private readonly IRuleFactory _ruleFactory;

    public string Report => _analysisReportInProgress.AsString();
    public int ReturnCode => _analysisReportInProgress.HasViolations() ? -1 : 0;


    public static Analysis PrepareFor(IReadOnlyList<XmlProject> xmlProjects, INScanSupport support)
    {
      var csharpWorkspaceModel = new CsharpWorkspaceModel(support, xmlProjects);
      var projects = csharpWorkspaceModel.LoadProjects();

      return new Analysis(new DotNetStandardSolution(projects,
          new PathCache(
            new DependencyPathFactory())),
        new PathRuleSet(), 
        new ProjectScopedRuleSet(), 
        new AnalysisReportInProgress(new PlainProjectPathFormat()),
        new RuleFactory());
    }

    public Analysis(
      ISolution solution, 
      IPathRuleSet pathRules,
      IProjectScopedRuleSet projectScopedRules,
      IAnalysisReportInProgress analysisReportInProgress, 
      IRuleFactory ruleFactory)
    {
      _solution = solution;
      _pathRules = pathRules;
      _projectScopedRules = projectScopedRules;
      _analysisReportInProgress = analysisReportInProgress;
      _ruleFactory = ruleFactory;
    }

    public void Run()
    {
      _solution.ResolveAllProjectsReferences();
      _solution.BuildCache();
      _solution.PrintDebugInfo();
      _solution.Check(_pathRules, _analysisReportInProgress);
      _solution.Check(_projectScopedRules, _analysisReportInProgress);
    }

    public void AddRules(IEnumerable<RuleUnionDto> rules)
    {
      foreach (var ruleDto in rules)
      {
        ruleDto.Switch(
          independentRule =>
          {
            var rule = _ruleFactory.CreateDependencyRuleFrom(ruleDto.IndependentRule);
            _pathRules.Add(rule);
          },
          correctNamespacesDto =>
          {
            var rule = _ruleFactory.CreateProjectScopedRuleFrom(correctNamespacesDto);
            _projectScopedRules.Add(rule);
          }, 
          noCricularUsingsDto =>
          {
            var rule = _ruleFactory.CreateProjectScopedRuleFrom(noCricularUsingsDto);
            _projectScopedRules.Add(rule);
          });
      }
    }
  }
}