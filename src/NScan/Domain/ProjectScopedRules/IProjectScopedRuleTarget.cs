using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.ProjectScopedRules
{
  public interface IProjectScopedRuleTarget
  {
    void AnalyzeFiles(IProjectFilesetScopedRule rule, IAnalysisReportInProgress report);
    bool HasProjectAssemblyNameMatching(Pattern pattern);
    void ValidateTargetFrameworkWith(ITargetFrameworkCheck targetFrameworkCheck,
      IAnalysisReportInProgress analysisReportInProgress);
  }
}