using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.Root;
using Xunit;

namespace TddXt.NScan.Specification.Domain.DependencyPathBasedRules
{
  public class JoinedDescribedConditionSpecification
  {
    
    [Fact]
    public void ShouldReturnsMatchBasedOnInnerConditionsMatchLogicalProduct()
    {
      //GIVEN
      var condition1 = Substitute.For<IDependencyCondition>();
      var condition2 = Substitute.For<IDependencyCondition>();
      var depending = AnyRoot.Root.Any.Instance<IProjectSearchResult>();
      var dependency = AnyRoot.Root.Any.Instance<IReferencedProject>();
      var condition1Result = AnyRoot.Root.Any.Boolean();
      var condition2Result = AnyRoot.Root.Any.Boolean();

      var joinedCondition = new JoinedDescribedCondition(condition1, condition2, AnyRoot.Root.Any.String());
      
      condition1.Matches(depending, dependency).Returns(condition1Result);
      condition2.Matches(depending, dependency).Returns(condition2Result);
      
      //WHEN
      var matches = joinedCondition.Matches(depending, dependency);

      //THEN
      matches.Should().Be(condition1Result && condition2Result);
    }

    [Fact]
    public void ShouldReturnsADescriptionItWasCreatedWith()
    {
      //GIVEN
      var initialDescription = AnyRoot.Root.Any.String();
      var condition = new JoinedDescribedCondition(
        AnyRoot.Root.Any.Instance<IDependencyCondition>(), 
        AnyRoot.Root.Any.Instance<IDependencyCondition>(),
        initialDescription);
      
      //WHEN
      var description = condition.Description();

      //THEN
      description.Should().Be(initialDescription);
    }
  }
}