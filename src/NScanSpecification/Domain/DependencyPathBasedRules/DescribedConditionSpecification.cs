using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.DependencyPathBasedRules
{
  public class DescribedConditionSpecification
  {
    [Fact]
    public void ShouldMatchWhenItsInnerConditionMatches()
    {
      //GIVEN
      var innerCondition = Substitute.For<IDependencyCondition>();
      var innerMatching = Any.Boolean();
      var condition = new DescribedCondition(innerCondition, Any.String());
      var depending = Any.Instance<IProjectSearchResult>();
      var dependency = Any.Instance<IReferencedProject>();

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
      var initialDescription = Any.String();
      var condition = new DescribedCondition(Any.Instance<IDependencyCondition>(), initialDescription);

      //WHEN
      var descriptionFromCondition = condition.Description();

      //THEN
      descriptionFromCondition.Should().Be(initialDescription);
    }
  }
}