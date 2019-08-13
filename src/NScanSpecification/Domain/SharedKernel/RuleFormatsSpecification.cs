using System.Data;
using FluentAssertions;
using NScan.SharedKernel.Ports;
using NScan.SharedKernel.SharedKernel;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.SharedKernel
{
  public class RuleFormatsSpecification
  {
    [Fact]
    public void ShouldProvideFormattedDescriptionOfNoCircularUsingsRuleDto()
    {
      //GIVEN
      var dto = Any.Instance<NoCircularUsingsRuleComplementDto>();
      //WHEN
      var text = RuleFormats.Format(dto);

      //THEN
      text.Should().Be($"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}");
    }

    [Fact]
    public void ShouldProvideFormattedDescriptionOfHasTargetFrameworkRuleDto()
    {
      //GIVEN
      var dto = Any.Instance<HasTargetFrameworkRuleComplementDto>();
      //WHEN
      var text = RuleFormats.Format(dto);

      //THEN
      text.Should().Be($"{dto.ProjectAssemblyNamePattern.Description()} {RuleNames.HasTargetFramework} {dto.TargetFramework}");
    }
    
    [Fact]
    public void ShouldProvideFormattedDescriptionOfCorrectNamespacesDto()
    {
      //GIVEN
      var dto = Any.Instance<CorrectNamespacesRuleComplementDto>();
      
      //WHEN
      var text = RuleFormats.Format(dto);

      //THEN
      text.Should().Be($"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName}");
    }
    
    [Fact]
    public void ShouldProvideFormattedDescriptionOfIndependentRuleDto()
    {
      //GIVEN
      var dto = Any.Instance<IndependentRuleComplementDto>();
      
      //WHEN
      var text = RuleFormats.Format(dto);

      //THEN
      text.Should().Be($"{dto.DependingPattern.Description()} {RuleNames.IndependentOf} {dto.DependencyType}:{dto.DependencyPattern.Pattern}");
    }
    
    [Fact]
    public void ShouldProvideFormattedDescriptionOfHasAttributesOnRuleDto()
    {
      //GIVEN
      var dto = Any.Instance<HasAttributesOnRuleComplementDto>();
      
      //WHEN
      var text = RuleFormats.Format(dto);

      //THEN
      text.Should().Be($"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName} " +
                       $"{dto.ClassNameInclusionPattern.Description()}:{dto.MethodNameInclusionPattern.Description()}");
    }
  }
}
