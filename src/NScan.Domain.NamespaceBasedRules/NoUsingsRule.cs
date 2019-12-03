using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.NamespaceBasedRules
{
  public class NoUsingsRule : INamespacesBasedRule
  {
    private readonly NoUsingsRuleComplementDto _ruleDto;

    public NoUsingsRule(NoUsingsRuleComplementDto ruleDto)
    {
      _ruleDto = ruleDto;
    }

    public string Description()
    {
      throw new System.NotImplementedException();
    }

    public void Evaluate(string projectAssemblyName, INamespacesDependenciesCache namespacesCache,
      IAnalysisReportInProgress report)
    {
    }
  }
}