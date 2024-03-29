﻿using NScan.DependencyPathBasedRules;
using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRulesSpecification;

public class JoinedDescribedConditionSpecification
{
  [Fact]
  public void ShouldReturnsMatchBasedOnInnerConditionsMatchLogicalProduct()
  {
    //GIVEN
    var condition1 = Substitute.For<IDependencyCondition>();
    var condition2 = Substitute.For<IDependencyCondition>();
    var depending = Any.Instance<IProjectSearchResult>();
    var dependency = Any.Instance<IDependencyPathBasedRuleTarget>();
    var condition1Result = Any.Boolean();
    var condition2Result = Any.Boolean();

    var joinedCondition = new JoinedDescribedCondition(condition1, condition2, Any.Instance<RuleDescription>());
      
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
    var conditionDescription = Any.Instance<RuleDescription>();
    var condition = new JoinedDescribedCondition(
      Any.Instance<IDependencyCondition>(), 
      Any.Instance<IDependencyCondition>(), 
      conditionDescription);
      
    //WHEN
    var description = condition.Description();

    //THEN
    description.Should().Be(description);
  }
}
