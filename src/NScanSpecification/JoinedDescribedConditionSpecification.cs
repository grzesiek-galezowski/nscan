using System;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.App;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification
{
  public class JoinedDescribedConditionSpecification
  {
    
    //bug rewrite UTs

    [Fact]
    public void ShouldHaveADescription()
    {
      //GIVEN
      var dependencyAssemblyNamePattern = Any.Instance<Glob.Glob>();
      var dependingAssemblyNamePattern = Any.Instance<Glob.Glob>();
      var condition = new JoinedDescribedCondition(new IsFollowingAssemblyCondition(), Any.Instance<IDescribedDependencyCondition>(), DependencyDescriptions.IndependentOf(
        dependingAssemblyNamePattern.Pattern,
        dependencyAssemblyNamePattern.Pattern));
      
      //WHEN
      var description = condition.Description();

      //THEN
      description.Should().Be(
        DependencyDescriptions.IndependentOf(
          dependingAssemblyNamePattern.Pattern,
          dependencyAssemblyNamePattern.Pattern));
    }
  }
}