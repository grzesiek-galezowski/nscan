using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.ProjectScopedRules
{
  public interface IProjectScopedRuleTarget
  {
    void AnalyzeFiles(IProjectScopedRule rule, IAnalysisReportInProgress report);
    bool HasProjectAssemblyNameMatching(Pattern pattern);
  }
}