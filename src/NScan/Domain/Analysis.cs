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
    private readonly ISolution _solution;
    private readonly IPathRuleSet _pathRules;
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
        new AnalysisReportInProgress(new PlainProjectPathFormat()),
        new RuleFactory());
    }

    public Analysis(ISolution solution, IPathRuleSet pathRules,
      IAnalysisReportInProgress analysisReportInProgress, IRuleFactory ruleFactory)
    {
      _solution = solution;
      _pathRules = pathRules;
      _analysisReportInProgress = analysisReportInProgress;
      _ruleFactory = ruleFactory;
    }

    public void Run()
    {
      _solution.ResolveAllProjectsReferences(_analysisReportInProgress);
      _solution.BuildCache();
      _solution.PrintDebugInfo();
      _solution.Check(_pathRules, _analysisReportInProgress);
    }

    public void AddRules(IEnumerable<RuleUnionDto> rules)
    {
      foreach (var ruleDto in rules)
      {
        var rule = _ruleFactory.CreateDependencyRuleFrom(ruleDto.Left);
        _pathRules.Add(rule);
      }
    }
  }
}