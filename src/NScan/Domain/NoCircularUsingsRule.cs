using System.Linq;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain
{
  public class NoCircularUsingsRule : INamespacesBasedRule
  {
    private readonly NoCircularUsingsRuleComplementDto _ruleDto;

    public NoCircularUsingsRule(NoCircularUsingsRuleComplementDto ruleDto)
    {
      _ruleDto = ruleDto;
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
        report.NamespacesBasedRuleViolation(Description(), projectAssemblyName, cycles);
      }
    }
  }
}