using TddXt.NScan.RuleInputData;

namespace TddXt.NScan.Domain
{
  public interface IRuleFactory
  {
    IDependencyRule CreateDependencyRuleFrom(IndependentRuleComplementDto independentRuleComplementDto);
    IProjectScopedRule CreateProjectScopedRuleFrom(CorrectNamespacesRuleComplementDto ruleDto);
    IProjectScopedRule CreateProjectScopedRuleFrom(NoCircularUsingsRuleComplementDto ruleDto);
  }
}