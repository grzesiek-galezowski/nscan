using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public interface ICSharpClass
{
  bool NameMatches(Pattern namePattern);
  void EvaluateDecorationWithAttributes(IAnalysisReportInProgress report, Pattern methodNameInclusionPattern, RuleDescription description);
}