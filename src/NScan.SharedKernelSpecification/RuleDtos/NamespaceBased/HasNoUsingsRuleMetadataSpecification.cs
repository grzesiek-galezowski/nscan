using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.SharedKernelSpecification.RuleDtos.NamespaceBased;

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
    text.Should().Be(new RuleDescription(
      $"{dto.ProjectAssemblyNamePattern.Text()} {HasNoUsingsRuleMetadata.HasNoUsings} from {dto.FromPattern.Text()} to {dto.ToPattern.Text()}"));
  }
}