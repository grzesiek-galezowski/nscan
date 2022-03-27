using System;
using FluentAssertions;
using NScan.NamespaceBasedRules;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.XFluentAssert.Api;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.NamespaceBasedRulesSpecification;

public class NamespaceNameSpecification
{
  [Fact]
  public void ShouldImplementEqualityBasedOnStringContent()
  {
    var value = Any.String().ToUpperInvariant();
    ObjectsOfType<NamespaceName>.ShouldHaveValueSemantics(
      new Func<NamespaceName>[]
      {
        () => new NamespaceName(value),
      },
      new Func<NamespaceName>[]
      {
        () => new NamespaceName(value.ToLowerInvariant()),
        () => new NamespaceName(Any.OtherThan(value))
      });
  }
    
  [Fact]
  public void ShouldCompareToOtherNames()
  {
    new NamespaceName("a").Should().BeLessThan(new NamespaceName("b"));
    new NamespaceName("b").Should().BeLessOrEqualTo(new NamespaceName("b"));
    new NamespaceName("b").Should().BeGreaterThan(new NamespaceName("a"));
    new NamespaceName("b").Should().BeGreaterOrEqualTo(new NamespaceName("b"));
  }
}