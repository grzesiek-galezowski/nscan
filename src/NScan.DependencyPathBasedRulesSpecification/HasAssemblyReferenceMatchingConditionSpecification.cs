﻿using FluentAssertions;
using GlobExpressions;
using NScan.DependencyPathBasedRules;
using NSubstitute;
using TddXt.AnyRoot;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.DependencyPathBasedRulesSpecification
{
  public class HasAssemblyReferenceMatchingConditionSpecification
  {
    [Fact]
    public void ShouldReturnResultOfQueryWhetherProjectHasAssembly()
    {
      //GIVEN
      var depending = Any.Instance<IProjectSearchResult>();
      var dependency = Substitute.For<IDependencyPathBasedRuleTarget>();
      var pattern = Any.Instance<Glob>();
      var dependencyResponse = Any.Boolean();
      var condition = new HasAssemblyReferenceMatchingCondition(pattern);

      dependency.HasAssemblyReferenceWithNameMatching(pattern).Returns(dependencyResponse);

      //WHEN
      var matches = condition.Matches(depending, dependency);

      //THEN
      matches.Should().Be(dependencyResponse);
    }
  }
}
