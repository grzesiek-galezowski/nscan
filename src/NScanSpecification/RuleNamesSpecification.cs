using FluentAssertions;
using Xunit;

namespace TddXt.NScan.Specification
{
  public class RuleNamesSpecification
  {
    [Fact]
    public void ShouldHaveIndependentOfRuleNameAsString()
    {
      RuleNames.IndependentOf.Should().Be("independentOf");
    }
  }
}