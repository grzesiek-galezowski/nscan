using System.Collections.Generic;
using MyTool.App;

namespace MyTool.CompositionRoot
{
  public class Analysis
  {
    private readonly ISolution _solution;
    private readonly IPathRuleSet _pathRules;
    private readonly IAnalysisReportInProgress _analysisReportInProgress;

    public Analysis(ISolution solution, IPathRuleSet pathRules, IAnalysisReportInProgress analysisReportInProgress)
    {
      _solution = solution;
      _pathRules = pathRules;
      _analysisReportInProgress = analysisReportInProgress;
    }

    public void Run()
    {
      _solution.ResolveAllProjectsReferences(_analysisReportInProgress);
      _solution.BuildCache();
      _solution.PrintDebugInfo();
      _solution.Check(_pathRules, _analysisReportInProgress);
    }

    public static Analysis Of(Dictionary<ProjectId, IDotNetProject> projects)
    {
      return new Analysis(new DotNetStandardSolution(projects, 
        new PathCache(
          new DependencyPathFactory())), new PathRuleSet(), new AnalysisReportInProgress()); //TODO expose the rule set or use method below?
    }

    public void DirectIndependentOfProject(ProjectId projectId, ProjectId dependent)
    {
      _pathRules.AddDirectIndependentOfProjectRule(projectId, dependent);
    }

    public string Report => _analysisReportInProgress.AsString();
  }
}