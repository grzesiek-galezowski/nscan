using System.Linq;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos;

namespace NScan.Domain.NamespaceBasedRules
{
  public class NoCircularUsingsRule : INamespacesBasedRule
  {
    private readonly NoCircularUsingsRuleComplementDto _ruleDto;
    private readonly INamespaceBasedRuleViolationFactory _ruleViolationFactory;

    public NoCircularUsingsRule(NoCircularUsingsRuleComplementDto ruleDto, INamespaceBasedRuleViolationFactory ruleViolationFactory)
    {
      _ruleDto = ruleDto;
      _ruleViolationFactory = ruleViolationFactory;
    }

    public string Description()
    {
      return $"{_ruleDto.ProjectAssemblyNamePattern.Description()} {_ruleDto.RuleName}";
    }

    public void Evaluate(
      string projectAssemblyName, 
      INamespacesDependenciesCache namespacesCache,
      IAnalysisReportInProgress report)
    {
      var cycles = namespacesCache.RetrieveCycles();
      if (cycles.Any())
      {
        report.Add(_ruleViolationFactory.NoCyclesRuleViolation(Description(), projectAssemblyName, cycles));
      }
    }
  }
}