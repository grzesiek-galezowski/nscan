using NScan.SharedKernel.RuleDtos.DependencyPathBased;

namespace NScan.DependencyPathBasedRules;

public class CreateDependencyBasedRuleVisitor : IPathBasedRuleDtoVisitor
{
  private readonly IDependencyBasedRuleFactory _dependencyBasedRuleFactory;
  private readonly IPathRuleSet _pathRules;

  public CreateDependencyBasedRuleVisitor(
    IDependencyBasedRuleFactory dependencyBasedRuleFactory,
    IPathRuleSet pathRules)
  {
    _dependencyBasedRuleFactory = dependencyBasedRuleFactory;
    _pathRules = pathRules;
  }

  public void Visit(IndependentRuleComplementDto dto)
  {
    var rule = _dependencyBasedRuleFactory.CreateDependencyRuleFrom(dto);
    _pathRules.Add(rule);
  }
}