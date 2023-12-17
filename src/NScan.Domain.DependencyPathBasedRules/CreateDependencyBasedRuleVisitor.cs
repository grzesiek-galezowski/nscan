using NScan.SharedKernel.RuleDtos.DependencyPathBased;

namespace NScan.DependencyPathBasedRules;

public class CreateDependencyBasedRuleVisitor(
  IDependencyBasedRuleFactory dependencyBasedRuleFactory,
  IPathRuleSet pathRules)
  : IPathBasedRuleDtoVisitor
{
  public void Visit(IndependentRuleComplementDto dto)
  {
    var rule = dependencyBasedRuleFactory.CreateDependencyRuleFrom(dto);
    pathRules.Add(rule);
  }
}
