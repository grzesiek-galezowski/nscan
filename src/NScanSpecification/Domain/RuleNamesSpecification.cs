using System;
using FluentAssertions;
using TddXt.NScan.Domain;
using Xunit;

namespace TddXt.NScan.Specification.Domain
{
  public class RuleNamesSpecification
  {
    [Fact]
    public void ShouldHaveRuleNames()
    {
      RuleNames.IndependentOf.Should().Be("independentOf");
      RuleNames.HasCorrectNamespaces.Should().Be("hasCorrectNamespaces");
    }
  }

}