using FluentAssertions;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.ReadingRules.Ports
{
  public class IndependentRuleMetadataSpecification
  {
    [Fact]
    public void ShouldAllowGettingRuleName()
    {
      IndependentRuleMetadata.IndependentOf.Should().Be("independentOf");
    }

    [Fact]
    public static void ShouldProvideFormattedDescriptionOfIndependentRuleDto()
    {
      //GIVEN
      var dto = Any.Instance<IndependentRuleComplementDto>();

      //WHEN
      var text = IndependentRuleMetadata.Format(dto);

      //THEN
      text.Should()
        .Be(
          $"{dto.DependingPattern.Description()} {IndependentRuleMetadata.IndependentOf} {dto.DependencyType}:{dto.DependencyPattern.Pattern}");
    }

  }
}