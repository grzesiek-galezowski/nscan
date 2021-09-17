﻿using FluentAssertions;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScanSpecification.ReadingRules.Ports
{
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
          $"{dto.ProjectAssemblyNamePattern.Description()} {dto.RuleName} " +
          $"{dto.ClassNameInclusionPattern.Description()}:{dto.MethodNameInclusionPattern.Description()}"));
    }
  }
}
