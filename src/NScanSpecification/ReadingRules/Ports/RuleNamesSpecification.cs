using FluentAssertions;
using NScan.SharedKernel.SharedKernel;
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
      RuleNames.HasAttributesOn.Should().Be("hasAttributesOn");
      RuleNames.HasTargetFramework.Should().Be("hasTargetFramework");
    }
  }

}