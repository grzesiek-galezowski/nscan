using FluentAssertions;
using Sprache;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.CompositionRoot;
using Xunit;
using static System.Environment;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.CompositionRoot
{
  public class ParseRuleSpecification
  {
    [Fact]
    public void ShouldParseDefaultRuleSyntax()
    {
      //GIVEN
      var depending = Any.String();
      var dependencyType = Any.String();
      var dependency = Any.String();
      var ruleName = Any.String();

      //WHEN
      var ruleDto = ParseRule.FromLine()
        .Parse($"{depending} {ruleName} {dependencyType}:{dependency}{NewLine}");

      //THEN
      ruleDto.DependingPattern.Should().Be(Pattern.WithoutExclusion(depending));
      ruleDto.RuleName.Should().Be(ruleName);
      ruleDto.DependencyType.Should().Be(dependencyType);
      ruleDto.DependencyPattern.Pattern.Should().Be(dependency);
    }

    [Fact]
    public void ShouldParseRuleSyntaxWithDependingException() //TODO will be new feature
    {
      //GIVEN
      var depending = Any.String();
      var dependencyType = Any.String();
      var dependency = Any.String();
      var ruleName = Any.String();
      var dependingException = Any.String();

      //WHEN
      var ruleDto = ParseRule.FromLine()
        .Parse($"{depending} except {dependingException} {ruleName} {dependencyType}:{dependency}{NewLine}");

      //THEN
      ruleDto.DependingPattern.Should().Be(Pattern.WithExclusion(depending, dependingException));
      ruleDto.RuleName.Should().Be(ruleName);
      ruleDto.DependencyType.Should().Be(dependencyType);
      ruleDto.DependencyPattern.Pattern.Should().Be(dependency);
    }

    [Fact]
    public void ShouldParseDefaultRuleSyntaxWithMoreThanOneSpace()
    {
      //GIVEN
      var depending = Any.String();
      var dependencyType = Any.String();
      var dependency = Any.String();
      var ruleName = Any.String();

      //WHEN
      var ruleDto = ParseRule.FromLine()
        .Parse($"{depending}  {ruleName}  {dependencyType}:{dependency}{NewLine}");

      //THEN
      ruleDto.DependencyType.Should().Be(dependencyType);
      ruleDto.DependencyPattern.Pattern.Should().Be(dependency);
      ruleDto.DependingPattern.Should().Be(Pattern.WithoutExclusion(depending));
      ruleDto.RuleName.Should().Be(ruleName);
    }
  }
}