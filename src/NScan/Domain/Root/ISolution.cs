using NScan.Domain.Domain.DependencyPathBasedRules;
using NScan.Domain.Domain.NamespaceBasedRules;
using NScan.Domain.Domain.ProjectScopedRules;
using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.Root
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