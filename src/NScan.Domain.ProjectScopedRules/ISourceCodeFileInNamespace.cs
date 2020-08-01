using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public interface ISourceCodeFileInNamespace
  {
    void CheckNamespacesCorrectness(IAnalysisReportInProgress report, string ruleDescription);
    void CheckMethodsHavingCorrectAttributes(
      IAnalysisReportInProgress report, 
      Pattern classNameInclusionPattern,
      Pattern methodNameInclusionPattern,
      string ruleDescription);
  }
}