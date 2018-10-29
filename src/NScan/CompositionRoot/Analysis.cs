using System.Collections.Generic;
using TddXt.NScan.App;
using TddXt.NScan.Xml;

namespace TddXt.NScan.CompositionRoot
{
  public class Analysis
  {
    private readonly ISolution _solution;
    private readonly IPathRuleSet _pathRules;
    private readonly IAnalysisReportInProgress _analysisReportInProgress;
    private readonly IRuleFactory _ruleFactory;

    public string Report => _analysisReportInProgress.AsString();
    public int ReturnCode => _analysisReportInProgress.HasViolations() ? -1 : 0; //todo 1 not -1?


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

    public static Analysis PrepareFor(IReadOnlyList<XmlProject> xmlProjects, ISupport support)
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


    public void IndependentOfPackage(string depending, string packageName)
    {
      _pathRules.Add(_ruleFactory.CreateIndependentOfPackageRule(depending, packageName));
    }
  }
}