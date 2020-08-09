using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRules
{
  public interface ISolutionForDependencyPathBasedRules
  {
    void ResolveAllProjectsReferences();
    void Check(IPathRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress);
    void BuildDependencyPathCache();
  }
}