using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public class CorrectNamespacesInFileCheck : ISourceCodeFileContentCheck
{
  public void ApplyTo(ISourceCodeFileInNamespace sourceCodeFile, RuleDescription description,
    IAnalysisReportInProgress report)
  {
    sourceCodeFile.CheckNamespacesCorrectness(report, description);
  }
}