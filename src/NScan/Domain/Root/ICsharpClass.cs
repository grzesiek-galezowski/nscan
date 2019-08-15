using NScan.Lib;
using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.Root
{
  public interface ICSharpClass
  {
    bool NameMatches(Pattern namePattern);
    void EvaluateDecorationWithAttributes(IAnalysisReportInProgress report, Pattern methodNameInclusionPattern, string ruleDescription);
  }
}