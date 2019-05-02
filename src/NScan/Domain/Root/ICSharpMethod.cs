using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.Root
{
  public interface ICSharpMethod
  {
    bool NameMatches(Pattern methodNameInclusionPattern);
    void EvaluateMethodsHavingCorrectAttributes(IAnalysisReportInProgress report, string parentClassName, string ruleDescription);
  }
}