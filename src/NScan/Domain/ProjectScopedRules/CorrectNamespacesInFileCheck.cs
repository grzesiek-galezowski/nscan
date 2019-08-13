using NScan.SharedKernel.SharedKernel;

namespace TddXt.NScan.Domain.ProjectScopedRules
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