using System.Collections.Generic;
using MyTool.App;

namespace MyTool.CompositionRoot
{
  public class Analysis
  {
    private readonly ISolution _solution;
    private readonly IPathRuleSet _pathRules;
    private readonly IAnalysisInProgressReport _analysisInProgressReport;

    public Analysis(ISolution solution, IPathRuleSet pathRules, IAnalysisInProgressReport analysisInProgressReport)
    {
      _solution = solution;
      _pathRules = pathRules;
      _analysisInProgressReport = analysisInProgressReport;
    }

    public void Run()
    {
      _solution.ResolveAllProjectsReferences(_analysisInProgressReport);
      _solution.BuildCaches();
      _solution.PrintDebugInfo();
      _solution.Check(_pathRules, _analysisInProgressReport);
    }

    public static Analysis Of(Dictionary<ProjectId, IDotNetProject> projects)
    {
      return new Analysis(new DotNetStandardSolution(projects), new PathRuleSet(), new AnalysisInProgressReport()); //TODO expose the rule set or use method below?
    }

    public void DirectIndependentOfProject(ProjectId projectId, ProjectId dependent)
    {
      _pathRules.AddDirectIndependentOfProjectRule(projectId, dependent);
    }

    public string Report => _analysisInProgressReport.AsString();
  }
}