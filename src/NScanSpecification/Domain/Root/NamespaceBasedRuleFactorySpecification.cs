using FluentAssertions;
using NScan.Domain.Root;
using NScan.NamespaceBasedRules;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using TddXt.XFluentAssert.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.Root
{
  public class NamespaceBasedRuleFactorySpecification
  {
    [Fact]
    public void ShouldCreateNoCircularDependenciesRule()
    {
      //GIVEN
      var violationFactory = Any.Instance<INamespaceBasedRuleViolationFactory>();
      var ruleFactory = new NamespaceBasedRuleFactory(violationFactory);
      var ruleDto = Any.Instance<NoCircularUsingsRuleComplementDto>();

      //WHEN
      var projectScopedRule = ruleFactory.CreateNamespacesBasedRuleFrom(ruleDto);

      //THEN
      projectScopedRule.Should().BeOfType<NoCircularUsingsRule>();
      projectScopedRule.Should().DependOn(ruleDto);
      projectScopedRule.Should().DependOn(violationFactory);
    }
    
    [Fact]
    public void ShouldCreateNoUsingsRule()
    {
      //GIVEN
      var ruleFactory = new NamespaceBasedRuleFactory(new RuleViolationFactory(new PlainReportFragmentsFormat()));
      var ruleDto = Any.Instance<NoUsingsRuleComplementDto>();

      //WHEN
      var projectScopedRule = ruleFactory.CreateNamespacesBasedRuleFrom(ruleDto);

      //THEN
      projectScopedRule.Should().BeOfType<NoUsingsRule>();
      projectScopedRule.Should().DependOn(ruleDto);
    }
  }
}