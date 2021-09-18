﻿using FluentAssertions;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScanSpecification.ReadingRules.Ports
{
  public class HasTargetFrameworkRuleMetadataSpecification
  {
    [Fact]
    public void ShouldAllowGettingRuleName()
    {
      HasTargetFrameworkRuleMetadata.HasTargetFramework.Should().Be("hasTargetFramework");
    }

    [Fact]
    public static void ShouldProvideFormattedDescriptionOfHasTargetFrameworkRuleDto()
    {
      //GIVEN
      var dto = Any.Instance<HasTargetFrameworkRuleComplementDto>();
      //WHEN
      var text = HasTargetFrameworkRuleMetadata.Format(dto);

      //THEN
      text.Should().Be(
        new RuleDescription(
          $"{dto.ProjectAssemblyNamePattern.Text()} " +
          $"{HasTargetFrameworkRuleMetadata.HasTargetFramework} " +
          $"{dto.TargetFramework}"));
    }
  }

}
