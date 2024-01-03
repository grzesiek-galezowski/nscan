using LanguageExt;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules;

public class SolutionForNamespaceBasedRules(Seq<INamespaceBasedRuleTarget> namespaceBasedRuleTargets)
  : ISolutionForNamespaceBasedRules
{
  public void Check(INamespacesBasedRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress)
  {
    ruleSet.Check(namespaceBasedRuleTargets, analysisReportInProgress);
  }

  public void BuildNamespacesCache()
  {
    foreach (var target in namespaceBasedRuleTargets)
    {
      target.RefreshNamespacesCache();
    }
  }
}
