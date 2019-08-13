using System;
using System.Linq;
using FluentAssertions;
using NScan.Lib;
using NScan.SharedKernel.SharedKernel;
using Sprache;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.ReadingRules.Adapters;
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
      ruleUnionDto.Accept(new IndependentRuleComplementDtoAssertion(independentRule =>
      {
        independentRule.DependingPattern.Should().Be(Pattern.WithoutExclusion(depending));
        independentRule.DependencyType.Should().Be(dependencyType);
        independentRule.DependencyPattern.Pattern.Should().Be(dependency);
        independentRule.RuleName.Should().Be(RuleNames.IndependentOf);
      }));
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
        .Parse(
          $"{depending} except {dependingException} {RuleNames.IndependentOf} {dependencyType}:{dependency}{Environment.NewLine}");

      //THEN
      ruleUnionDto.Accept(new IndependentRuleComplementDtoAssertion(independentRule =>
        {
          independentRule.DependingPattern.Should().Be(Pattern.WithExclusion(depending, dependingException));
          independentRule.DependencyType.Should().Be(dependencyType);
          independentRule.DependencyPattern.Pattern.Should().Be(dependency);
          independentRule.RuleName.Should().Be(RuleNames.IndependentOf);
        }));
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
      ruleUnionDto.Accept(new IndependentRuleComplementDtoAssertion(independentRule =>
      {
        independentRule.DependingPattern.Should().Be(Pattern.WithoutExclusion(depending));
        independentRule.DependencyType.Should().Be(dependencyType);
        independentRule.DependencyPattern.Pattern.Should().Be(dependency);
        independentRule.RuleName.Should().Be(RuleNames.IndependentOf);
      }));
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

      ruleUnionDto.Accept(new CorrectNamespacesRuleComplementDtoAssertion(dto =>
      {
        dto.RuleName.Should().Be(RuleNames.HasCorrectNamespaces);
        dto.ProjectAssemblyNamePattern.Should().Be(Pattern.WithoutExclusion(depending));
      }));
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
      ruleUnionDto.Accept(new NoCircularUsingsRuleComplementDtoAssertion(dto =>
      {
        dto.RuleName.Should().Be(RuleNames.HasNoCircularUsings);
        dto.ProjectAssemblyNamePattern.Should().Be(Pattern.WithoutExclusion(depending));
      }));
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
      ruleUnionDto.Accept(new NoCircularUsingsRuleComplementDtoAssertion(dto =>
      {
        dto.RuleName.Should().Be(RuleNames.HasNoCircularUsings);
        dto.ProjectAssemblyNamePattern.Should().Be(Pattern.WithoutExclusion(depending));
      }));
      ruleUnionDto.RuleName.Should().Be(RuleNames.HasNoCircularUsings);
    }

    [Fact]
    public void ShouldParseHasAttributesOnRuleDefinitionMultipleTimes()
    {
      //GIVEN
      var depending = Any.String();
      var classPattern = Any.String();
      var methodPattern = Any.String();

      //WHEN
      var ruleUnionDtos = ParseRule.FromLine().Many().Parse(
        $"{depending} {RuleNames.HasAttributesOn} {classPattern}:{methodPattern}{Environment.NewLine}" +
        $"{depending} {RuleNames.HasAttributesOn} {classPattern}:{methodPattern}{Environment.NewLine}"
      ).ToList();

      //THEN
      ruleUnionDtos.Count.Should().Be(2);
      var rule1Dto = ruleUnionDtos.First();
      rule1Dto.Accept(new HasAttributesOnRuleComplementAssertion(dto =>
      {
        dto.RuleName.Should().Be(RuleNames.HasAttributesOn);
        dto.ClassNameInclusionPattern.Description().Should().Be(classPattern);
        dto.MethodNameInclusionPattern.Description().Should().Be(methodPattern);
        dto.ProjectAssemblyNamePattern.Description().Should().Be(depending);
      }));
      rule1Dto.RuleName.Should().Be(RuleNames.HasAttributesOn);
    }

    [Fact]
    public void ShouldParseHasTargetFrameworkDefinitionMultipleTimes()
    {
      //GIVEN
      var depending = Any.String();
      var frameworkName = Any.String();

      //WHEN
      var ruleUnionDtos = ParseRule.FromLine().Many().Parse(
        $"{depending} {RuleNames.HasTargetFramework} {frameworkName}{Environment.NewLine}" +
        $"{depending} {RuleNames.HasTargetFramework} {frameworkName}{Environment.NewLine}"
      ).ToList();

      //THEN
      ruleUnionDtos.Count.Should().Be(2);
      var rule1Dto = ruleUnionDtos.First();
      rule1Dto.Accept(new HasTargetFrameworkAssertion(dto =>
      {
        dto.RuleName.Should().Be(RuleNames.HasTargetFramework);
        dto.ProjectAssemblyNamePattern.Description().Should().Be(depending);
        dto.TargetFramework.Should().Be(frameworkName);
      }));
      rule1Dto.RuleName.Should().Be(RuleNames.HasTargetFramework);
    }
  }
}