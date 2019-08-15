using NScan.Lib;
using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.ProjectScopedRules
{
  public interface IProjectScopedRuleTarget
  {
    void AnalyzeFiles(IProjectFilesetScopedRule rule, IAnalysisReportInProgress report);
    bool HasProjectAssemblyNameMatching(Pattern pattern);
    void ValidateTargetFrameworkWith(ITargetFrameworkCheck targetFrameworkCheck,
      IAnalysisReportInProgress analysisReportInProgress);
  }
}