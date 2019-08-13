using NScan.Lib;
using NScan.SharedKernel.SharedKernel;

namespace TddXt.NScan.Domain.Root
{
  public interface ICSharpClass
  {
    bool NameMatches(Pattern namePattern);
    void EvaluateDecorationWithAttributes(IAnalysisReportInProgress report, Pattern methodNameInclusionPattern, string ruleDescription);
  }
}