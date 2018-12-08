namespace TddXt.NScan.Domain
{
  public interface ISolution
  {
    void ResolveAllProjectsReferences();
    void PrintDebugInfo();
    void Check(IPathRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress); //BACKLOG maybe later extract more generic interface for other rules
    void Check(IProjectScopedRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress);
    void BuildCache();
  }
}