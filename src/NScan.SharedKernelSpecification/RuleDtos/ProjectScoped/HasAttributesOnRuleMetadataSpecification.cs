﻿using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.SharedKernelSpecification.RuleDtos.ProjectScoped;

public class HasAttributesOnRuleMetadataSpecification
{
  [Fact]
  public void ShouldAllowGettingRuleName()
  {
    HasAttributesOnRuleMetadata.HasAttributesOn.Should().Be("hasAttributesOn");
  }

  [Fact]
  public static void ShouldProvideFormattedDescriptionOfHasAttributesOnRuleDto()
  {
    //GIVEN
    var dto = Any.Instance<HasAttributesOnRuleComplementDto>();
      
    //WHEN
    var text = HasAttributesOnRuleMetadata.Format(dto);

    //THEN
    text.Should().Be(
      new RuleDescription(
        $"{dto.ProjectAssemblyNamePattern.Text()} {dto.RuleName} " +
        $"{dto.ClassNameInclusionPattern.Text()}:{dto.MethodNameInclusionPattern.Text()}"));
  }
}