using FluentAssertions;
using NScan.DependencyPathBasedRules;
using NScan.SharedKernel;
using NSubstitute;
using TddXt.AnyRoot;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.DependencyPathBasedRulesSpecification;

public class DescribedConditionSpecification
{
  [Fact]
  public void ShouldMatchWhenItsInnerConditionMatches()
  {
    //GIVEN
    var innerCondition = Substitute.For<IDependencyCondition>();
    var innerMatching = Any.Boolean();
    var condition = new DescribedCondition(innerCondition, Any.Instance<RuleDescription>());
    var depending = Any.Instance<IProjectSearchResult>();
    var dependency = Any.Instance<IDependencyPathBasedRuleTarget>();

    innerCondition.Matches(depending, dependency).Returns(innerMatching);

    //WHEN
    var matches = condition.Matches(depending, dependency);

    //THEN
    matches.Should().Be(innerMatching);
  }

  [Fact]
  public void ShouldReturnTheDescriptionItWasCreatedWith()
  {
    //GIVEN
    var initialDescription = Any.Instance<RuleDescription>();
    var condition = new DescribedCondition(Any.Instance<IDependencyCondition>(), initialDescription);

    //WHEN
    var descriptionFromCondition = condition.Description();

    //THEN
    descriptionFromCondition.Should().Be(initialDescription);
  }
}