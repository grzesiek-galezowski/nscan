using System;
using System.Linq;
using FluentAssertions;
using Sprache;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.ReadingRules.Adapters;
using TddXt.NScan.ReadingRules.Ports;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.ReadingRules.Adapters
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
        .Parse($"{depending} {RuleNames.IndependentOf} {dependencyType}:{dependency}{Environment.NewLine}");

      //THEN
      ruleUnionDto.Switch(independentRule =>
      {
        independentRule.DependingPattern.Should().Be(Pattern.WithoutExclusion(depending));
        independentRule.DependencyType.Should().Be(dependencyType);
        independentRule.DependencyPattern.Pattern.Should().Be(dependency);
        independentRule.RuleName.Should().Be(RuleNames.IndependentOf);
      }, FailWhen<CorrectNamespacesRuleComplementDto>(), FailWhen<NoCircularUsingsRuleComplementDto>());
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
        .Parse($"{depending} except {dependingException} {RuleNames.IndependentOf} {dependencyType}:{dependency}{Environment.NewLine}");

      //THEN
      ruleUnionDto.Switch(independentRule =>
      {
        independentRule.DependingPattern.Should().Be(Pattern.WithExclusion(depending, dependingException));
        independentRule.DependencyType.Should().Be(dependencyType);
        independentRule.DependencyPattern.Pattern.Should().Be(dependency);
        independentRule.RuleName.Should().Be(RuleNames.IndependentOf);
      }, FailWhen<CorrectNamespacesRuleComplementDto>(), FailWhen<NoCircularUsingsRuleComplementDto>());
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
        .Parse($"{depending}  {RuleNames.IndependentOf}  {dependencyType}:{dependency}{Environment.NewLine}");

      //THEN
      ruleUnionDto.Switch(independentRule =>
      {
        independentRule.DependingPattern.Should().Be(Pattern.WithoutExclusion(depending));
        independentRule.DependencyType.Should().Be(dependencyType);
        independentRule.DependencyPattern.Pattern.Should().Be(dependency);
        independentRule.RuleName.Should().Be(RuleNames.IndependentOf);
      }, 
        FailWhen<CorrectNamespacesRuleComplementDto>(), 
        FailWhen<NoCircularUsingsRuleComplementDto>());
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

      ruleUnionDto.Switch(
        FailWhen<IndependentRuleComplementDto>(),
        dto =>
        {
          dto.RuleName.Should().Be(RuleNames.HasCorrectNamespaces);
          dto.ProjectAssemblyNamePattern.Should().Be(Pattern.WithoutExclusion(depending));
        },
        FailWhen<NoCircularUsingsRuleComplementDto>());
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
      ruleUnionDto.Switch(
        FailWhen<IndependentRuleComplementDto>(),
        FailWhen<CorrectNamespacesRuleComplementDto>(),
        dto =>
        {
          dto.RuleName.Should().Be(RuleNames.HasNoCircularUsings);
          dto.ProjectAssemblyNamePattern.Should().Be(Pattern.WithoutExclusion(depending));
        });
      ruleUnionDto.RuleName.Should().Be(RuleNames.HasNoCircularUsings);
    }

    [Fact]
    public void ShouldParseNoCircularUsingsRuleDefinitionMultipleTimes()
    {
      //GIVEN
      var depending = Any.String();

      //WHEN
      var ruleUnionDtos = ParseRule.FromLine().Many().Parse(
        $"{depending} {RuleNames.HasNoCircularUsings}{Environment.NewLine}" + 
        $"{depending} {RuleNames.HasNoCircularUsings}{Environment.NewLine}"
        ).ToList();

      //THEN
      ruleUnionDtos.Count.Should().Be(2);
      var ruleUnionDto = ruleUnionDtos.First();
      ruleUnionDto.Switch(
        FailWhen<IndependentRuleComplementDto>(),
        FailWhen<CorrectNamespacesRuleComplementDto>(),
        dto =>
        {
          dto.RuleName.Should().Be(RuleNames.HasNoCircularUsings);
          dto.ProjectAssemblyNamePattern.Should().Be(Pattern.WithoutExclusion(depending));
        });
      ruleUnionDto.RuleName.Should().Be(RuleNames.HasNoCircularUsings);
    }

    private static Action<T> FailWhen<T>()
    {
      return dto => { Assert.False(true);};
    }

  }
}