using FluentAssertions;
using GlobExpressions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.NScan.Domain;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.SharedKernel;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
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