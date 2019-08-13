using NScan.Lib;
using NScan.SharedKernel.SharedKernel;

namespace TddXt.NScan.Domain.ProjectScopedRules
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