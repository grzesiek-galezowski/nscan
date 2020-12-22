using System.IO;
using FluentAssertions;
using NScan.Lib;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.ReadingRules.Ports
{
  public class PatternSpecification
  {
    [Fact]
    public void ShouldMatchStringsWithoutExclusion()
    {
      //GIVEN
      var pattern = Pattern.WithoutExclusion("*a*");

      //WHEN
      var result = pattern.IsMatch("abc");

      //THEN
      result.Should().BeTrue();
    }

    [Fact]
    public void ShouldNotMatchStringsWithoutExclusion()
    {
      //GIVEN
      var pattern = Pattern.WithoutExclusion("*a*");

      //WHEN
      var result = pattern.IsMatch("bc");

      //THEN
      result.Should().BeFalse();
    }

    [Fact]
    public void ShouldReportMatchWhenMainPatternMatchesAndExclusionDoesNot()
    {
      //GIVEN
      var pattern = Pattern.WithExclusion("*", "*Specification*");

      //WHEN
      var result = pattern.IsMatch("bc");

      //THEN
      result.Should().BeTrue();
    }

    [Fact]
    public void ShouldNotReportMatchWhenMainPatternMatchesAndExclusionAsWell()
    {
      //GIVEN
      var pattern = Pattern.WithExclusion("*", "*Specification*");

      //WHEN
      var result = pattern.IsMatch("WhateverSpecification");

      //THEN
      result.Should().BeFalse();
    }

    [Fact]
    public void ShouldDescribeOnlyInclusionPatternWhenThereIsNoExclusionPattern()
    {
      //GIVEN
      var inclusionPattern = Any.String();
      var pattern = Pattern.WithoutExclusion(inclusionPattern);
      //WHEN
      var description = pattern.Description();
      //THEN
      description.Should().Be(inclusionPattern);
    }

    [Fact]
    public void ShouldDescribeBothInclusionAndExclusionPatternsWhenBothArePresent()
    {
      //GIVEN
      var inclusionPattern = Any.String();
      var exclusionPattern = Any.String();
      var pattern = Pattern.WithExclusion(inclusionPattern, exclusionPattern);
      //WHEN
      var description = pattern.Description();
      
      //THEN
      description.Should().Be(inclusionPattern + " except " + exclusionPattern);
    }
  }
}
