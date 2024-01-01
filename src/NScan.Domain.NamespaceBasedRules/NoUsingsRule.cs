using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.NamespaceBasedRules;

public class NoUsingsRule(NoUsingsRuleComplementDto dto, INamespaceBasedRuleViolationFactory ruleViolationFactory)
  : INamespacesBasedRule
{
  public RuleDescription Description()
  {
    return HasNoUsingsRuleMetadata.Format(dto);
  }

  public void Evaluate(AssemblyName projectAssemblyName,
    INamespacesDependenciesCache namespacesCache,
    IAnalysisReportInProgress report)
  {
    var paths = namespacesCache.RetrievePathsBetween(dto.FromPattern, dto.ToPattern);
    if (paths.Any())
    {
      report.Add(
        ruleViolationFactory.NoUsingsRuleViolation(Description(), projectAssemblyName, paths));
    }
  }
}
