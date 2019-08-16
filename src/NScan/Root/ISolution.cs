using NScan.Domain.DependencyPathBasedRules;
using NScan.Domain.NamespaceBasedRules;
using NScan.Domain.ProjectScopedRules;
using NScan.SharedKernel;

namespace NScan.Domain.Root
{
  public interface ISolution
  {
    void ResolveAllProjectsReferences();
    void PrintDebugInfo();
    void Check(IPathRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress);
    void Check(IProjectScopedRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress);
    void Check(INamespacesBasedRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress);
    void BuildCache();
  }
}