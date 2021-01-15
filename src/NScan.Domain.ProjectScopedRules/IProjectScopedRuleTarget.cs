using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public interface IProjectScopedRuleTarget
  {
    void AnalyzeFiles(IProjectFilesetScopedRule rule, IAnalysisReportInProgress report);
    bool HasProjectAssemblyNameMatching(Pattern pattern);
    void ValidateProperty(IPropertyCheck propertyCheck,
      IAnalysisReportInProgress analysisReportInProgress);
  }

}