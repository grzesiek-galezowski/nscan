using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.NamespaceBasedRules
{
  public class NoUsingsRule : INamespacesBasedRule
  {
    private readonly NoUsingsRuleComplementDto _dto;

    public NoUsingsRule(NoUsingsRuleComplementDto dto)
    {
      _dto = dto;
    }

    public string Description()
    {
      return HasNoUsingsRuleMetadata.Format(_dto);
    }

    public void Evaluate(
      string projectAssemblyName, 
      INamespacesDependenciesCache namespacesCache,
      IAnalysisReportInProgress report)
    {
    }
  }
}