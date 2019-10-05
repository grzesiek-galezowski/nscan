using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public interface ISourceCodeFileInNamespace
  {
    void EvaluateNamespacesCorrectness(IAnalysisReportInProgress report, string ruleDescription);
    void EvaluateMethodsHavingCorrectAttributes(
      IAnalysisReportInProgress report, 
      Pattern classNameInclusionPattern,
      Pattern methodNameInclusionPattern,
      string ruleDescription);
  }
}