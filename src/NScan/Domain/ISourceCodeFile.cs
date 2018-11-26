namespace TddXt.NScan.Domain
{
  public interface ISourceCodeFile
  {
    void EvaluateNamespacesCorrectness(IAnalysisReportInProgress report);
  }
  //bug only for projects where assembly name matches pattern
}