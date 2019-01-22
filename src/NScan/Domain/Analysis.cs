using System.Collections.Generic;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingRules.Ports;
using TddXt.NScan.ReadingSolution;
using TddXt.NScan.ReadingSolution.Ports;

namespace TddXt.NScan.Domain
{
  public class Analysis
  {
    public const int ReturnCodeOk = 0;
    public const int ReturnCodeAnalysisFailed = -1;

    private readonly ISolution _solution;
    private readonly IPathRuleSet _pathRules;
    private readonly IProjectScopedRuleSet _projectScopedRules;
    private readonly INamespacesBasedRuleSet _namespacesBasedRuleSet;
    private readonly IAnalysisReportInProgress _analysisReportInProgress;
    private readonly IRuleFactory _ruleFactory;

    public string Report => _analysisReportInProgress.AsString();
    public int ReturnCode => _analysisReportInProgress.HasViolations() ? -1 : 0;


    public static Analysis PrepareFor(IReadOnlyList<XmlProject> xmlProjects, INScanSupport support)
    {
      var plainReportFragmentsFormat = new PlainReportFragmentsFormat();
      var csharpWorkspaceModel = new CsharpWorkspaceModel(support, xmlProjects, new RuleViolationFactory(plainReportFragmentsFormat));
      var projects = csharpWorkspaceModel.CreateProjectsDictionary();

      return new Analysis(new DotNetStandardSolution(projects,
          new PathCache(
            new DependencyPathFactory())),
        new PathRuleSet(), 
        new ProjectScopedRuleSet(), 
        new NamespacesBasedRuleSet(),
        new AnalysisReportInProgress(),
        new RuleFactory());
    }

    public Analysis(ISolution solution,
      IPathRuleSet pathRules,
      IProjectScopedRuleSet projectScopedRules,
      INamespacesBasedRuleSet namespacesBasedRuleSet,
      IAnalysisReportInProgress analysisReportInProgress,
      IRuleFactory ruleFactory)
    {
      _solution = solution;
      _pathRules = pathRules;
      _projectScopedRules = projectScopedRules;
      _namespacesBasedRuleSet = namespacesBasedRuleSet;
      _analysisReportInProgress = analysisReportInProgress;
      _ruleFactory = ruleFactory;
    }

    public void Run()
    {
      _solution.ResolveAllProjectsReferences();
      _solution.BuildCache();
      //_solution.PrintDebugInfo();
      _solution.Check(_pathRules, _analysisReportInProgress);
      _solution.Check(_projectScopedRules, _analysisReportInProgress);
      _solution.Check(_namespacesBasedRuleSet, _analysisReportInProgress);
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
            var rule = _ruleFactory.CreateNamespacesBasedRuleFrom(noCricularUsingsDto);
            _namespacesBasedRuleSet.Add(rule);
          });
      }
    }
  }
}