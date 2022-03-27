using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public interface ISourceCodeFileInNamespace
{
  void CheckNamespacesCorrectness(IAnalysisReportInProgress report, RuleDescription description);
  void CheckMethodsHavingCorrectAttributes(
    IAnalysisReportInProgress report, 
    Pattern classNameInclusionPattern,
    Pattern methodNameInclusionPattern, 
    RuleDescription description);
}