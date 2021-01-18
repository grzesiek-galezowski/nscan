using FluentAssertions;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScanSpecification.ReadingRules.Ports
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
      var dto = Any.Instance<NoUsingsRuleComplementDto>();
      
      //WHEN
      var text = HasNoUsingsRuleMetadata.Format(dto);

      //THEN
      text.Should().Be($"{dto.ProjectAssemblyNamePattern.Description()} {HasNoUsingsRuleMetadata.HasNoUsings} from {dto.FromPattern.Description()} to {dto.ToPattern.Description()}");
    }
  }
}
