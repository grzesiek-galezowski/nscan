namespace TddXt.NScan.App
{
  public interface ISolution
  {
    void ResolveAllProjectsReferences(IAnalysisReportInProgress analysisReportInProgress);
    void PrintDebugInfo();
    void Check(IPathRuleSet pathRuleSet, IAnalysisReportInProgress analysisReportInProgress); //BACKLOG maybe later extract more generic interface for other rules
    void BuildCache();
  }
}