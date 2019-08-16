using FluentAssertions;
using GlobExpressions;
using NScan.Domain.DependencyPathBasedRules;
using NScan.Domain.Root;
using NSubstitute;
using TddXt.AnyRoot;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.DependencyPathBasedRules
{
  public class HasAssemblyReferenceMatchingConditionSpecification
  {
    [Fact]
    public void ShouldReturnResultOfQueryWhetherProjectHasAssembly()
    {
      //GIVEN
      var depending = Any.Instance<IProjectSearchResult>();
      var dependency = Substitute.For<IReferencedProject>();
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