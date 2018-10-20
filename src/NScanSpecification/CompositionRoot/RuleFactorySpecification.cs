using FluentAssertions;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.CompositionRoot;
using TddXt.XFluentAssert.Root;
using Xunit;

namespace TddXt.NScan.Specification.CompositionRoot
{
  public class RuleFactorySpecification
  {
    [Fact]
    public void ShouldCreateIndependentOfProjectRuleWithPassedIds()
    {
      //GIVEN
      var ruleFactory = new RuleFactory();
      
      //WHEN
      var dependingId = Root.Any.String();
      var dependencyId = Root.Any.String();
      var rule = ruleFactory.CreateIndependentOfProjectRule(dependingId, dependencyId);

      //THEN
      rule.GetType().Should().Be<IndependentOfProjectRule>();
      rule.Should().DependOn(dependingId);
      rule.Should().DependOn(dependencyId);
    }
  }
}