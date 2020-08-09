using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRules
{
  public interface ISolutionForDependencyPathBasedRules
  {
    void Check(IPathRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress);
    void BuildDependencyPathCache();
  }
}