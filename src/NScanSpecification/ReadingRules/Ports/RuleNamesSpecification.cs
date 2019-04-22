using FluentAssertions;
using TddXt.NScan.ReadingRules.Ports;
using Xunit;

namespace TddXt.NScan.Specification.ReadingRules.Ports
{
  public class RuleNamesSpecification
  {
    [Fact]
    public void ShouldHaveRuleNames()
    {
      RuleNames.IndependentOf.Should().Be("independentOf");
      RuleNames.HasCorrectNamespaces.Should().Be("hasCorrectNamespaces");
      RuleNames.HasNoCircularUsings.Should().Be("hasNoCircularUsings");
      RuleNames.HasAnnotationsOn.Should().Be("hasAnnotationsOn");
    }
  }

}