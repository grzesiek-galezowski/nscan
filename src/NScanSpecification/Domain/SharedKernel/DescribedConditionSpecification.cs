using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.SharedKernel;
using Xunit;

namespace TddXt.NScan.Specification.Domain.SharedKernel
{
  public class DescribedConditionSpecification
  {
    [Fact]
    public void ShouldMatchWhenItsInnerConditionMatches()
    {
      //GIVEN
      var innerCondition = Substitute.For<IDependencyCondition>();
      var innerMatching = AnyRoot.Root.Any.Boolean();
      var condition = new DescribedCondition(innerCondition, AnyRoot.Root.Any.String());
      var depending = AnyRoot.Root.Any.Instance<IProjectSearchResult>();
      var dependency = AnyRoot.Root.Any.Instance<IReferencedProject>();

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
      var initialDescription = AnyRoot.Root.Any.String();
      var condition = new DescribedCondition(AnyRoot.Root.Any.Instance<IDependencyCondition>(), initialDescription);

      //WHEN
      var descriptionFromCondition = condition.Description();

      //THEN
      descriptionFromCondition.Should().Be(initialDescription);
    }
  }
}