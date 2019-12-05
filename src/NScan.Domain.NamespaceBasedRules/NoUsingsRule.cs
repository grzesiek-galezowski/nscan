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
      //bug component test
      return $"{_dto.ProjectAssemblyNamePattern.Description()} {_dto.RuleName} from {_dto.FromPattern.Description()} to {_dto.ToPattern.Description()}"; //bug
    }

    public void Evaluate(string projectAssemblyName, INamespacesDependenciesCache namespacesCache,
      IAnalysisReportInProgress report)
    {
    }
  }
}