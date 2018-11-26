namespace TddXt.NScan.Domain
{
  public interface IProjectScopedRuleTarget
  {
    void AnalyzeFiles(IProjectScopedRule rule, IAnalysisReportInProgress report);
    bool HasProjectAssemblyNameMatching(Pattern pattern);
  }
}