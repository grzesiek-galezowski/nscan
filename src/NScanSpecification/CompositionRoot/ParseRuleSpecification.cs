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

      //WHEN
      var ruleDto = ParseRule.FromLine()
        .Parse($"{depending} {RuleNames.IndependentOf} {dependencyType}:{dependency}{NewLine}");

      //THEN
      ruleDto.DependingPattern.Should().Be(Pattern.WithoutExclusion(depending));
      ruleDto.IndependentRuleComplement.DependencyType.Should().Be(dependencyType);
      ruleDto.IndependentRuleComplement.DependencyPattern.Pattern.Should().Be(dependency);
      ruleDto.RuleName.Should().Be(RuleNames.IndependentOf);
    }

    [Fact]
    public void ShouldParseRuleSyntaxWithDependingException() //TODO will be new feature
    {
      //GIVEN
      var depending = Any.String();
      var dependencyType = Any.String();
      var dependency = Any.String();
      var dependingException = Any.String();

      //WHEN
      var ruleDto = ParseRule.FromLine()
        .Parse($"{depending} except {dependingException} {RuleNames.IndependentOf} {dependencyType}:{dependency}{NewLine}");

      //THEN
      ruleDto.DependingPattern.Should().Be(Pattern.WithExclusion(depending, dependingException));
      ruleDto.IndependentRuleComplement.DependencyType.Should().Be(dependencyType);
      ruleDto.IndependentRuleComplement.DependencyPattern.Pattern.Should().Be(dependency);
      ruleDto.RuleName.Should().Be(RuleNames.IndependentOf);
    }

    [Fact]
    public void ShouldParseDefaultRuleSyntaxWithMoreThanOneSpace()
    {
      //GIVEN
      var depending = Any.String();
      var dependencyType = Any.String();
      var dependency = Any.String();

      //WHEN
      var ruleDto = ParseRule.FromLine()
        .Parse($"{depending}  {RuleNames.IndependentOf}  {dependencyType}:{dependency}{NewLine}");

      //THEN
      ruleDto.IndependentRuleComplement.DependencyType.Should().Be(dependencyType);
      ruleDto.IndependentRuleComplement.DependencyPattern.Pattern.Should().Be(dependency);
      ruleDto.DependingPattern.Should().Be(Pattern.WithoutExclusion(depending));
      ruleDto.RuleName.Should().Be(RuleNames.IndependentOf);
    }
    
    [Fact]
    public void ShouldParseNamespacesIntactRuleDefinition()
    {
      //GIVEN
      var depending = Any.String();

      //WHEN
      var ruleDto = ParseRule.FromLine()
        .Parse($"{depending}  hasCorrectNamespaces");

      //THEN
      ruleDto.IndependentRuleComplement.Should().BeNull(); //bug maybe!
      ruleDto.CorrectNamespacesRuleComplement.Should().NotBeNull();
      ruleDto.RuleName.Should().Be(RuleNames.HasCorrectNamespaces);
    }
  }
}