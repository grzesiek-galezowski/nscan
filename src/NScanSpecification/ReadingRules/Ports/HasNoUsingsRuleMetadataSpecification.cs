using FluentAssertions;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using TddXt.AnyRoot;
using Xunit;

namespace TddXt.NScan.Specification.ReadingRules.Ports
{
  public class HasNoUsingsRuleMetadataSpecification
  {
    [Fact]
    public void ShouldAllowGettingRuleName()
    {
      HasNoUsingsRuleMetadata.HasNoUsings.Should().Be("hasNoUsings");
    }

    [Fact]
    public static void ShouldProvideFormattedDescriptionOfIndependentRuleDto()
    {
      //GIVEN
      var dto = Root.Any.Instance<NoUsingsRuleComplementDto>();
      
      //WHEN
      var text = HasNoUsingsRuleMetadata.Format(dto);

      //THEN
      text.Should().Be($"{dto.ProjectAssemblyNamePattern.Description()} {HasNoUsingsRuleMetadata.HasNoUsings} from {dto.FromPattern.Description()} to {dto.ToPattern.Description()}");
    }
  }
}