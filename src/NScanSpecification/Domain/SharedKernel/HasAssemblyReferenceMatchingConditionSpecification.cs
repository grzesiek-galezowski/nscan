using FluentAssertions;
using GlobExpressions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.Root;
using Xunit;

namespace TddXt.NScan.Specification.Domain.SharedKernel
{
  public class HasAssemblyReferenceMatchingConditionSpecification
  {
    [Fact]
    public void ShouldReturnResultOfQueryWhetherProjectHasAssembly()
    {
      //GIVEN
      var depending = AnyRoot.Root.Any.Instance<IProjectSearchResult>();
      var dependency = Substitute.For<IReferencedProject>();
      var pattern = AnyRoot.Root.Any.Instance<Glob>();
      var dependencyResponse = AnyRoot.Root.Any.Boolean();
      var condition = new HasAssemblyReferenceMatchingCondition(pattern);

      dependency.HasAssemblyReferenceWithNameMatching(pattern).Returns(dependencyResponse);

      //WHEN
      var matches = condition.Matches(depending, dependency);

      //THEN
      matches.Should().Be(dependencyResponse);
    }
  }
}