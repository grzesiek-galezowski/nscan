using System;
using System.Linq;
using FluentAssertions;
using Sprache;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.CompositionRoot;
using TddXt.NScan.Domain;
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
      var ruleUnionDto = ParseRule.FromLine()
        .Parse($"{depending} {RuleNames.IndependentOf} {dependencyType}:{dependency}{NewLine}");

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
      var depending = Any.String();
      var dependencyType = Any.String();
      var dependency = Any.String();
      var dependingException = Any.String();

      //WHEN
      var ruleUnionDto = ParseRule.FromLine()
        .Parse($"{depending} except {dependingException} {RuleNames.IndependentOf} {dependencyType}:{dependency}{NewLine}");

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
      var depending = Any.String();
      var dependencyType = Any.String();
      var dependency = Any.String();

      //WHEN
      var ruleUnionDto = ParseRule.FromLine()
        .Parse($"{depending}  {RuleNames.IndependentOf}  {dependencyType}:{dependency}{NewLine}");

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
      var depending = Any.String();

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
      var depending = Any.String();

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
      var depending = Any.String();

      //WHEN
      var ruleUnionDtos = ParseRule.FromLine().Many().Parse(
        $"{depending} {RuleNames.HasNoCircularUsings}{NewLine}" + 
        $"{depending} {RuleNames.HasNoCircularUsings}{NewLine}"
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