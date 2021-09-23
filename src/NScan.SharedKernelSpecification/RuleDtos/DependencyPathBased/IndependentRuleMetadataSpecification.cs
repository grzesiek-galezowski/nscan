using FluentAssertions;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.SharedKernelSpecification.RuleDtos.DependencyPathBased
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
      text.Should().Be(new RuleDescription(
          $"{dto.DependingPattern.Text()} {IndependentRuleMetadata.IndependentOf} {dto.DependencyType}:{dto.DependencyPattern.Pattern}"
          ));
    }

  }
}
