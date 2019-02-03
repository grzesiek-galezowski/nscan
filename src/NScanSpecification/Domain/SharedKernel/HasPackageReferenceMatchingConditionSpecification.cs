using FluentAssertions;
using GlobExpressions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.Root;
using Xunit;

namespace TddXt.NScan.Specification.Domain.SharedKernel
{
  public class HasPackageReferenceMatchingConditionSpecification
  {
    [Fact]
    public void ShouldMatchDependingOnWhetherTheDependencyHasMatchingPackageReference()
    {
      //GIVEN
      var packagePattern = AnyRoot.Root.Any.Instance<Glob>();
      var condition = new HasPackageReferenceMatchingCondition(packagePattern);
      var depending = AnyRoot.Root.Any.Instance<IProjectSearchResult>();
      var dependency = Substitute.For<IReferencedProject>();
      var dependencyAnswer = AnyRoot.Root.Any.Boolean();

      dependency.HasPackageReferenceMatching(packagePattern).Returns(dependencyAnswer);

      //WHEN
      var matches = condition.Matches(depending, dependency);

      //THEN
      matches.Should().Be(dependencyAnswer);
    }
  }
}