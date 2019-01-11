using System;
using System.Linq;
using FluentAssertions;
using Sprache;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.ReadingRules.Adapters;
using TddXt.NScan.ReadingRules.Ports;
using Xunit;

namespace TddXt.NScan.Specification.ReadingRules.Adapters
{
  public class ParseRuleSpecification
  {
    [Fact]
    public void ShouldParseDefaultRuleSyntax()
    {
      //GIVEN
      var depending = Root.Any.String();
      var dependencyType = Root.Any.String();
      var dependency = Root.Any.String();

      //WHEN
      var ruleUnionDto = ParseRule.FromLine()
        .Parse($"{depending} {RuleNames.IndependentOf} {dependencyType}:{dependency}{Environment.NewLine}");

      //THEN
      ruleUnionDto.IndependentRule.DependingPattern.Should().Be(Pattern.WithoutExclusion(depending));
      ruleUnionDto.IndependentRule.DependencyType.Should().Be(dependencyType);
      ruleUnionDto.IndependentRule.DependencyPattern.Pattern.Should().Be(dependency);
      ruleUnionDto.IndependentRule.RuleName.Should().Be(RuleNames.IndependentOf);
      ruleUnionDto.RuleName.Should().Be(RuleNames.IndependentOf);
    }

    [Fact]
    public void ShouldParseRuleSyntaxWithDependingException()
    {
      //GIVEN
      var depending = Root.Any.String();
      var dependencyType = Root.Any.String();
      var dependency = Root.Any.String();
      var dependingException = Root.Any.String();

      //WHEN
      var ruleUnionDto = ParseRule.FromLine()
        .Parse($"{depending} except {dependingException} {RuleNames.IndependentOf} {dependencyType}:{dependency}{Environment.NewLine}");

      //THEN
      ruleUnionDto.IndependentRule.DependingPattern.Should().Be(Pattern.WithExclusion(depending, dependingException));
      ruleUnionDto.IndependentRule.DependencyType.Should().Be(dependencyType);
      ruleUnionDto.IndependentRule.DependencyPattern.Pattern.Should().Be(dependency);
      ruleUnionDto.IndependentRule.RuleName.Should().Be(RuleNames.IndependentOf);
      ruleUnionDto.RuleName.Should().Be(RuleNames.IndependentOf);
    }

    [Fact]
    public void ShouldParseDefaultRuleSyntaxWithMoreThanOneSpace()
    {
      //GIVEN
      var depending = Root.Any.String();
      var dependencyType = Root.Any.String();
      var dependency = Root.Any.String();

      //WHEN
      var ruleUnionDto = ParseRule.FromLine()
        .Parse($"{depending}  {RuleNames.IndependentOf}  {dependencyType}:{dependency}{Environment.NewLine}");

      //THEN
      ruleUnionDto.IndependentRule.DependencyType.Should().Be(dependencyType);
      ruleUnionDto.IndependentRule.DependencyPattern.Pattern.Should().Be(dependency);
      ruleUnionDto.IndependentRule.DependingPattern.Should().Be(Pattern.WithoutExclusion(depending));
      ruleUnionDto.IndependentRule.RuleName.Should().Be(RuleNames.IndependentOf);
      ruleUnionDto.RuleName.Should().Be(RuleNames.IndependentOf);
    }

    [Fact]
    public void ShouldParseNamespacesIntactRuleDefinition()
    {
      //GIVEN
      var depending = Root.Any.String();

      //WHEN
      var ruleUnionDto = ParseRule.FromLine().Parse($"{depending}  {RuleNames.HasCorrectNamespaces}");

      //THEN
      ruleUnionDto.IndependentRule.Should().BeNull(); //bug maybe!
      ruleUnionDto.CorrectNamespacesRule.Should().NotBeNull();
      ruleUnionDto.CorrectNamespacesRule.RuleName.Should().Be(RuleNames.HasCorrectNamespaces);
      ruleUnionDto.CorrectNamespacesRule.ProjectAssemblyNamePattern.Should()
        .Be(Pattern.WithoutExclusion(depending));
      ruleUnionDto.RuleName.Should().Be(RuleNames.HasCorrectNamespaces);
    }

    [Fact]
    public void ShouldParseNoCircularUsingsRuleDefinition()
    {
      //GIVEN
      var depending = Root.Any.String();

      //WHEN
      var ruleUnionDto = ParseRule.FromLine().Parse($"{depending} {RuleNames.HasNoCircularUsings}");

      //THEN
      ruleUnionDto.IndependentRule.Should().BeNull(); //bug maybe!
      ruleUnionDto.NoCircularUsingsRule.Should().NotBeNull();
      ruleUnionDto.NoCircularUsingsRule.RuleName.Should().Be(RuleNames.HasNoCircularUsings);
      ruleUnionDto.RuleName.Should().Be(RuleNames.HasNoCircularUsings);
      ruleUnionDto.NoCircularUsingsRule.ProjectAssemblyNamePattern.Should()
        .Be(Pattern.WithoutExclusion(depending));
    }

    [Fact]
    public void ShouldParseNoCircularUsingsRuleDefinitionMultipleTimes()
    {
      //GIVEN
      var depending = Root.Any.String();

      //WHEN
      var ruleUnionDtos = ParseRule.FromLine().Many().Parse(
        $"{depending} {RuleNames.HasNoCircularUsings}{Environment.NewLine}" + 
        $"{depending} {RuleNames.HasNoCircularUsings}{Environment.NewLine}"
        );

      //THEN
      ruleUnionDtos.Count().Should().Be(2);
      var ruleUnionDto = ruleUnionDtos.First();
      ruleUnionDto.IndependentRule.Should().BeNull(); //bug maybe!
      ruleUnionDto.NoCircularUsingsRule.Should().NotBeNull();
      ruleUnionDto.NoCircularUsingsRule.RuleName.Should().Be(RuleNames.HasNoCircularUsings);
      ruleUnionDto.RuleName.Should().Be(RuleNames.HasNoCircularUsings);
      ruleUnionDto.NoCircularUsingsRule.ProjectAssemblyNamePattern.Should()
        .Be(Pattern.WithoutExclusion(depending));
    }
  }
}