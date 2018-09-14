using System.Collections.Generic;
using MyTool.App;
using MyTool.Xml;

namespace MyTool.CompositionRoot
{
  public class Analysis
  {
    private readonly ISolution _solution;
    private readonly IPathRuleSet _pathRules;
    private readonly IAnalysisReportInProgress _analysisReportInProgress;
    private readonly IRuleFactory _ruleFactory;

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

    public static Analysis PrepareFor(List<XmlProject> xmlProjects, ISupport support)
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

    public void IndependentOfProject(string depending, string dependent)
    {
      _pathRules.Add(_ruleFactory.CreateIndependentOfProjectRule(depending, dependent));
    }

    public string Report => _analysisReportInProgress.AsString();
    public int ReturnCode => _analysisReportInProgress.HasViolations() ? -1 : 0; //todo 1 not -1?
  }
}