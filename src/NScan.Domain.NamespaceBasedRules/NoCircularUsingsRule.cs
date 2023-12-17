using System.Linq;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.NamespaceBasedRules;

public class NoCircularUsingsRule(
  NoCircularUsingsRuleComplementDto ruleDto,
  INamespaceBasedRuleViolationFactory ruleViolationFactory)
  : INamespacesBasedRule
{
  public RuleDescription Description()
  {
    return HasNoCircularUsingsRuleMetadata.Format(ruleDto);
  }

  public void Evaluate(AssemblyName projectAssemblyName,
    INamespacesDependenciesCache namespacesCache,
    IAnalysisReportInProgress report)
  {
    var cycles = namespacesCache.RetrieveCycles();
    if (cycles.Any())
    {
      report.Add(
        ruleViolationFactory.NoCyclesRuleViolation(Description(), projectAssemblyName, cycles));
    }
  }
}
