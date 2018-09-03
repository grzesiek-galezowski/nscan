namespace MyTool.App
{
  public interface ISolution
  {
    void ResolveAllProjectsReferences(IAnalysisInProgressReport analysisInProgressReport);
    void PrintDebugInfo();
    void Check(IPathRuleSet pathRuleSet, IAnalysisInProgressReport analysisInProgressReport); //BACKLOG maybe later extract more generic interface for other rules
    void BuildCaches();
  }
}