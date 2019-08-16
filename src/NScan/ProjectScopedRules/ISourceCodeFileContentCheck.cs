using NScan.SharedKernel;

namespace NScan.Domain.ProjectScopedRules
{
  public interface ISourceCodeFileContentCheck
  {
    void ApplyTo(ISourceCodeFileInNamespace sourceCodeFile, string ruleDescription,
      IAnalysisReportInProgress report);
  }
}