using System.Collections.Generic;
using NScan.NamespaceBasedRules;
using NScan.SharedKernel;

namespace NScan.Domain
{
  public class SolutionForNamespaceBasedRules : ISolutionForNamespaceBasedRules //bug move
  {
    private readonly IReadOnlyList<INamespaceBasedRuleTarget> _namespaceBasedRuleTargets;

    public SolutionForNamespaceBasedRules(IReadOnlyList<INamespaceBasedRuleTarget> namespaceBasedRuleTargets)
    {
      _namespaceBasedRuleTargets = namespaceBasedRuleTargets;
    }

    public void Check(INamespacesBasedRuleSet ruleSet, IAnalysisReportInProgress analysisReportInProgress)
    {
      ruleSet.Check(_namespaceBasedRuleTargets, analysisReportInProgress);
    }

    public void BuildNamespacesCache()
    {
      foreach (var target in _namespaceBasedRuleTargets)
      {
        target.RefreshNamespacesCache();
      }
    }
  }
}