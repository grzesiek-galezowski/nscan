using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public interface ICSharpMethod
  {
    bool NameMatches(Pattern methodNameInclusionPattern);
    void EvaluateMethodsHavingCorrectAttributes(IAnalysisReportInProgress report, string parentClassName, RuleDescription description);
  }
}
