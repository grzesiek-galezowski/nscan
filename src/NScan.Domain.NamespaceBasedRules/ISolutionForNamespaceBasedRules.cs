using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules;

public interface ISolutionForNamespaceBasedRules
{
  void Check(INamespacesBasedRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress);
  void BuildNamespacesCache();
}