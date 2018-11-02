namespace TddXt.NScan.CompositionRoot
{
  public interface IRuleFactory
  {
    IDependencyRule CreateDependencyRuleFrom(RuleDto ruleDto);
  }
}