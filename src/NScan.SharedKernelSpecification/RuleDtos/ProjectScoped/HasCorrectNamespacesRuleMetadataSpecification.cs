using FluentAssertions;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.SharedKernelSpecification.RuleDtos.ProjectScoped
{
  public class HasCorrectNamespacesRuleMetadataSpecification
  {
    [Fact]
    public void ShouldAllowGettingRuleName()
    {
      HasCorrectNamespacesRuleMetadata.HasCorrectNamespaces.Should().Be("hasCorrectNamespaces");
    }

    [Fact]
    public static void ShouldProvideFormattedDescriptionOfCorrectNamespacesDto()
    {
      //GIVEN
      var dto = Any.Instance<CorrectNamespacesRuleComplementDto>();
      
      //WHEN
      var text = HasCorrectNamespacesRuleMetadata.Format(dto);

      //THEN
      text.Should().Be(new RuleDescription($"{dto.ProjectAssemblyNamePattern.Text()} {dto.RuleName}"));
    }
  }
}
