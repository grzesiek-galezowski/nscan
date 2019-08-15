using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.ProjectScopedRules
{
  public interface ISourceCodeFileContentCheck
  {
    void ApplyTo(ISourceCodeFileInNamespace sourceCodeFile, string ruleDescription,
      IAnalysisReportInProgress report);
  }
}