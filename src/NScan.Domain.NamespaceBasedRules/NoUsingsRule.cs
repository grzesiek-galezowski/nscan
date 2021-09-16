using System.Linq;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.NamespaceBasedRules
{
  public class NoUsingsRule : INamespacesBasedRule
  {
    private readonly NoUsingsRuleComplementDto _dto;
    private readonly INamespaceBasedRuleViolationFactory _ruleViolationFactory;

    public NoUsingsRule(NoUsingsRuleComplementDto dto, INamespaceBasedRuleViolationFactory ruleViolationFactory)
    {
      _dto = dto;
      _ruleViolationFactory = ruleViolationFactory;
    }

    public RuleDescription Description()
    {
      return new RuleDescription(HasNoUsingsRuleMetadata.Format(_dto));
    }

    public void Evaluate(
      string projectAssemblyName,
      INamespacesDependenciesCache namespacesCache,
      IAnalysisReportInProgress report)
    {
      var paths = namespacesCache.RetrievePathsBetween(_dto.FromPattern, _dto.ToPattern);
      if (paths.Any())
      {
        report.Add(
          _ruleViolationFactory.NoUsingsRuleViolation(
            Description().Value, 
            projectAssemblyName, paths));
      }
    }
  }
}
