using FluentAssertions;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.ReadingRules.Ports
{
  public class HasNoCircularUsingsRuleMetadataSpecification
  {
    [Fact]
    public void ShouldAllowGettingRuleName()
    {
      HasNoCircularUsingsRuleMetadata.HasNoCircularUsings.Should().Be("hasNoCircularUsings");
    }

    [Fact]
    public static void ShouldProvideFormattedDescriptionOfNoCircularUsingsRuleDto()
    {
      //GIVEN
      var dto = Any.Instance<NoCircularUsingsRuleComplementDto>();
      //WHEN
      var text = HasNoCircularUsingsRuleMetadata.Format(dto);

      //THEN
      text.Should().Be($"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}");
    }
  }
}