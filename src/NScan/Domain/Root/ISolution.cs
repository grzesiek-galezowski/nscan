using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.ProjectScopedRules;
using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.Root
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