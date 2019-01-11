using TddXt.NScan.RuleInputData;

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

    public void Evaluate(INamespacesDependenciesCache namespacesCache, IAnalysisReportInProgress report)
    {
      //bug throw new NotImplementedException();
    }
  }
}