using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public interface ISourceCodeFileContentCheck
  {
    void ApplyTo(ISourceCodeFileInNamespace sourceCodeFile, string ruleDescription,
      IAnalysisReportInProgress report);
  }
}