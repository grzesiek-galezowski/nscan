using NScan.Lib;

namespace NScan.LibSpecification;

public class PatternSpecification
{
  [Fact]
  public void ShouldMatchStringsWithoutExclusion()
  {
    //GIVEN
    var pattern = Pattern.WithoutExclusion("*a*");

    //WHEN
    var result = pattern.IsMatchedBy("abc");

    //THEN
    result.Should().BeTrue();
  }

  [Fact]
  public void ShouldNotMatchStringsWithoutExclusion()
  {
    //GIVEN
    var pattern = Pattern.WithoutExclusion("*a*");

    //WHEN
    var result = pattern.IsMatchedBy("bc");

    //THEN
    result.Should().BeFalse();
  }

  [Fact]
  public void ShouldReportMatchWhenMainPatternMatchesAndExclusionDoesNot()
  {
    //GIVEN
    var pattern = Pattern.WithExclusion("*", "*Specification*");

    //WHEN
    var result = pattern.IsMatchedBy("bc");

    //THEN
    result.Should().BeTrue();
  }

  [Fact]
  public void ShouldNotReportMatchWhenMainPatternMatchesAndExclusionAsWell()
  {
    //GIVEN
    var pattern = Pattern.WithExclusion("*", "*Specification*");

    //WHEN
    var result = pattern.IsMatchedBy("WhateverSpecification");

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
    var description = pattern.Text();
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
    var description = pattern.Text();
    //THEN
    description.Should().Be(inclusionPattern + " except " + exclusionPattern);
  }

  [Fact]
  //Because it's nice to be able to see this info in the debugger
  public void ShouldBeConvertibleToStringWhichContainsTheSameInformationAsItsText()
  {
    //GIVEN
    var pattern = Any.Instance<Pattern>();
      
    //THEN
    pattern.ToString().Should().Be(pattern.Text());
  }
}
