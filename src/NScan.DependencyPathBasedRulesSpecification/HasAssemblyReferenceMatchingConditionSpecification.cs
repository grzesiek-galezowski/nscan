using GlobExpressions;
using NScan.DependencyPathBasedRules;

namespace NScan.DependencyPathBasedRulesSpecification;

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
