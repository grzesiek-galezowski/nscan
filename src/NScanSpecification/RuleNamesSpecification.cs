using FluentAssertions;
using Xunit;

namespace TddXt.NScan.Specification
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