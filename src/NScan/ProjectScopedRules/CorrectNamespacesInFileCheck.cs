using NScan.SharedKernel;

namespace NScan.Domain.ProjectScopedRules
{
  public class CorrectNamespacesInFileCheck : ISourceCodeFileContentCheck
  {
    public void ApplyTo(ISourceCodeFileInNamespace sourceCodeFile, string ruleDescription,
      IAnalysisReportInProgress report)
    {
      sourceCodeFile.EvaluateNamespacesCorrectness(report, ruleDescription);
    }
  }
}