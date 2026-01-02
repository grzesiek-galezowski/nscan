using NScan.NamespaceBasedRules;

namespace NScan.NamespaceBasedRulesSpecification;

public class NamespaceNameSpecification
{
  [Fact]
  public void ShouldImplementEqualityBasedOnStringContent()
  {
    var value = Any.String().ToUpperInvariant();
    ObjectsOfType<NamespaceName>.ShouldHaveValueSemantics(
      [
        () => new NamespaceName(value)
      ],
      [
        () => new NamespaceName(value.ToLowerInvariant()),
        () => new NamespaceName(Any.OtherThan(value))
      ]);
  }
    
  [Fact]
  public void ShouldCompareToOtherNames()
  {
    new NamespaceName("a").Should().BeLessThan(new NamespaceName("b"));
    new NamespaceName("b").Should().BeLessThanOrEqualTo(new NamespaceName("b"));
    new NamespaceName("b").Should().BeGreaterThan(new NamespaceName("a"));
    new NamespaceName("b").Should().BeGreaterThanOrEqualTo(new NamespaceName("b"));
  }
}
