using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public class CorrectNamespacesInFileCheck : ISourceCodeFileContentCheck
  {
    public void ApplyTo(ISourceCodeFileInNamespace sourceCodeFile, string ruleDescription,
      IAnalysisReportInProgress report)
    {
      sourceCodeFile.CheckNamespacesCorrectness(report, ruleDescription);
    }
  }
}