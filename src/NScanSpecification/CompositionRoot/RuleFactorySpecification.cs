using FluentAssertions;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.CompositionRoot;
using TddXt.XFluentAssert.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

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
      var dependingId = Any.String();
      var dependencyId = Any.String();
      var rule = ruleFactory.CreateIndependentOfProjectRule(dependingId, dependencyId);

      //THEN
      rule.GetType().Should().Be<IndependentRule>();
      rule.Should().DependOn(dependingId);
      rule.Should().DependOn(dependencyId);
    }

    [Fact]
    public void ShouldCreateIndependentOfPackageRuleWithPassedIds()
    {
      //GIVEN
      var ruleFactory = new RuleFactory();
      
      //WHEN
      var dependingId = Any.String();
      var dependencyId = Any.String();
      var rule = ruleFactory.CreateIndependentOfPackageRule(dependingId, dependencyId);

      //THEN
      rule.GetType().Should().Be<IndependentOfPackageRule>();
      rule.Should().DependOn(dependingId);
      rule.Should().DependOn(dependencyId);
    }
  }
}