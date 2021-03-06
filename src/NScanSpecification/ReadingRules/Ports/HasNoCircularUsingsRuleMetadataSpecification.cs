﻿using FluentAssertions;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScanSpecification.ReadingRules.Ports
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
