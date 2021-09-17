using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public interface ISourceCodeFileContentCheck
  {
    void ApplyTo(
      ISourceCodeFileInNamespace sourceCodeFile, 
      RuleDescription description,
      IAnalysisReportInProgress report);
  }
}
