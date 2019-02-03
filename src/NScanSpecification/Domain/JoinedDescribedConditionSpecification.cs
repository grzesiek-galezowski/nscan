using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.Root;
using TddXt.NScan.Domain.SharedKernel;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
{
  public class JoinedDescribedConditionSpecification
  {
    
    [Fact]
    public void ShouldReturnsMatchBasedOnInnerConditionsMatchLogicalProduct()
    {
      //GIVEN
      var condition1 = Substitute.For<IDependencyCondition>();
      var condition2 = Substitute.For<IDependencyCondition>();
      var depending = Any.Instance<IProjectSearchResult>();
      var dependency = Any.Instance<IReferencedProject>();
      var condition1Result = Any.Boolean();
      var condition2Result = Any.Boolean();

      var joinedCondition = new JoinedDescribedCondition(condition1, condition2, Any.String());
      
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
      var initialDescription = Any.String();
      var condition = new JoinedDescribedCondition(
        Any.Instance<IDependencyCondition>(), 
        Any.Instance<IDependencyCondition>(),
        initialDescription);
      
      //WHEN
      var description = condition.Description();

      //THEN
      description.Should().Be(initialDescription);
    }
  }
}