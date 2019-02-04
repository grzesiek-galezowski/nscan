﻿using FluentAssertions;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.ReadingRules.Ports;
using TddXt.XFluentAssert.Root;
using Xunit;

namespace TddXt.NScan.Specification.ReadingRules.Ports
{
  public class PatternSpecification
  {
    [Fact]
    public void ShouldBehaveLikeValueObject()
    {
      typeof(Pattern).Should().HaveValueSemantics();
    }

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
      var inclusionPattern = Root.Any.String();
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
      var inclusionPattern = Root.Any.String();
      var exclusionPattern = Root.Any.String();
      var pattern = Pattern.WithExclusion(inclusionPattern, exclusionPattern);
      //WHEN
      var description = pattern.Description();
      
      //THEN
      description.Should().Be(inclusionPattern + " except " + exclusionPattern);
    }

  }
}