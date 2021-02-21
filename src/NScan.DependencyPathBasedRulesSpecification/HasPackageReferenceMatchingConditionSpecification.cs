using FluentAssertions;
using GlobExpressions;
using NScan.DependencyPathBasedRules;
using NSubstitute;
using TddXt.AnyRoot;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.DependencyPathBasedRulesSpecification
{
  public class HasPackageReferenceMatchingConditionSpecification
  {
    [Fact]
    public void ShouldMatchDependingOnWhetherTheDependencyHasMatchingPackageReference()
    {
      //GIVEN
      var packagePattern = Any.Instance<Glob>();
      var condition = new HasPackageReferenceMatchingCondition(packagePattern);
      var depending = Any.Instance<IProjectSearchResult>();
      var dependency = Substitute.For<IDependencyPathBasedRuleTarget>();
      var dependencyAnswer = Any.Boolean();

      dependency.HasPackageReferenceMatching(packagePattern).Returns(dependencyAnswer);

      //WHEN
      var matches = condition.Matches(depending, dependency);

      //THEN
      matches.Should().Be(dependencyAnswer);
    }
  }
}
