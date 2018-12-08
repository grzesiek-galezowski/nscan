namespace TddXt.NScan.Domain
{
  public interface ISourceCodeFile
  {
    void EvaluateNamespacesCorrectness(IAnalysisReportInProgress report, string ruleDescription);
  }
  //bug only for projects where assembly name matches pattern
}