using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.Domain
{
  public interface ICSharpMethod
  {
    bool NameMatches(Pattern methodNameInclusionPattern);
    void EvaluateMethodsHavingCorrectAttributes(IAnalysisReportInProgress report, string parentClassName, string ruleDescription);
  }
}