using System;
using System.Linq;
using FluentAssertions;
using NScan.Adapter.ReadingRules;
using NScan.Lib;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using NScanSpecification.Lib.AutomationLayer;
using Sprache;
using TddXt.AnyRoot.Strings;
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
      var ruleUnionDto = ParserRulePreface.Then(ParseDependencyPathBasedRule.Complement)
        .Parse($"{depending} {IndependentRuleMetadata.IndependentOf} {dependencyType}:{dependency}{Environment.NewLine}");

      //THEN
      ruleUnionDto.Accept(new IndependentRuleComplementDtoAssertion(independentRule =>
      {
        independentRule.DependingPattern.Should().Be(Pattern.WithoutExclusion(depending));
        independentRule.DependencyType.Should().Be(dependencyType);
        independentRule.DependencyPattern.Pattern.Should().Be(dependency);
        independentRule.RuleName.Should().Be(IndependentRuleMetadata.IndependentOf);
      }));
      ruleUnionDto.RuleName.Should().Be(IndependentRuleMetadata.IndependentOf);
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
      var ruleUnionDto = ParserRulePreface.Then(ParseDependencyPathBasedRule.Complement)
				.Parse(
          $"{depending} except {dependingException} {IndependentRuleMetadata.IndependentOf} {dependencyType}:{dependency}{Environment.NewLine}");

      //THEN
      ruleUnionDto.Accept(new IndependentRuleComplementDtoAssertion(independentRule =>
        {
          independentRule.DependingPattern.Should().Be(Pattern.WithExclusion(depending, dependingException));
          independentRule.DependencyType.Should().Be(dependencyType);
          independentRule.DependencyPattern.Pattern.Should().Be(dependency);
          independentRule.RuleName.Should().Be(IndependentRuleMetadata.IndependentOf);
        }));
      ruleUnionDto.RuleName.Should().Be(IndependentRuleMetadata.IndependentOf);
    }

    [Fact]
    public void ShouldParseDefaultRuleSyntaxWithMoreThanOneSpace()
    {
      //GIVEN
      var depending = Any.String();
      var dependencyType = Any.String();
      var dependency = Any.String();

      //WHEN
      var ruleUnionDto = ParserRulePreface.Then(ParseDependencyPathBasedRule.Complement)
				.Parse($"{depending}  {IndependentRuleMetadata.IndependentOf}  {dependencyType}:{dependency}{Environment.NewLine}");

      //THEN
      ruleUnionDto.Accept(new IndependentRuleComplementDtoAssertion(independentRule =>
      {
        independentRule.DependingPattern.Should().Be(Pattern.WithoutExclusion(depending));
        independentRule.DependencyType.Should().Be(dependencyType);
        independentRule.DependencyPattern.Pattern.Should().Be(dependency);
        independentRule.RuleName.Should().Be(IndependentRuleMetadata.IndependentOf);
      }));
      ruleUnionDto.RuleName.Should().Be(IndependentRuleMetadata.IndependentOf);
    }

    [Fact]
    public void ShouldParseNamespacesIntactRuleDefinition()
    {
      //GIVEN
      var depending = Any.String();

      //WHEN
      var ruleUnionDto = ParserRulePreface.Then(ParseProjectScopedRule.Complement).Parse(
	      $"{depending}  {HasCorrectNamespacesRuleMetadata.HasCorrectNamespaces}");

      //THEN

      ruleUnionDto.Accept(new CorrectNamespacesRuleComplementDtoAssertion(dto =>
      {
        dto.RuleName.Should().Be(HasCorrectNamespacesRuleMetadata.HasCorrectNamespaces);
        dto.ProjectAssemblyNamePattern.Should().Be(Pattern.WithoutExclusion(depending));
      }));
      ruleUnionDto.RuleName.Should().Be(HasCorrectNamespacesRuleMetadata.HasCorrectNamespaces);
    }

    [Fact]
    public void ShouldParseNoCircularUsingsRuleDefinition()
    {
      //GIVEN
      var depending = Any.String();

      //WHEN
      var ruleUnionDto = ParserRulePreface.Then(ParseNamespaceBasedRule.Complement).Parse($"{depending} {HasNoCircularUsingsRuleMetadata.HasNoCircularUsings}");

      //THEN
      ruleUnionDto.Accept(new NoCircularUsingsRuleComplementDtoAssertion(dto =>
      {
        dto.RuleName.Should().Be(HasNoCircularUsingsRuleMetadata.HasNoCircularUsings);
        dto.ProjectAssemblyNamePattern.Should().Be(Pattern.WithoutExclusion(depending));
      }));
      ruleUnionDto.RuleName.Should().Be(HasNoCircularUsingsRuleMetadata.HasNoCircularUsings);
    }

    [Fact]
    public void ShouldParseNoCircularUsingsRuleDefinitionMultipleTimes()
    {
      //GIVEN
      var depending = Any.String();

      //WHEN
      var ruleUnionDtos = ParserRulePreface.Then(ParseNamespaceBasedRule.Complement).Many().Parse(
        $"{depending} {HasNoCircularUsingsRuleMetadata.HasNoCircularUsings}{Environment.NewLine}" +
        $"{depending} {HasNoCircularUsingsRuleMetadata.HasNoCircularUsings}{Environment.NewLine}"
      ).ToList();

      //THEN
      ruleUnionDtos.Count.Should().Be(2);
      var ruleUnionDto = ruleUnionDtos.First();
      ruleUnionDto.Accept(new NoCircularUsingsRuleComplementDtoAssertion(dto =>
      {
        dto.RuleName.Should().Be(HasNoCircularUsingsRuleMetadata.HasNoCircularUsings);
        dto.ProjectAssemblyNamePattern.Should().Be(Pattern.WithoutExclusion(depending));
      }));
      ruleUnionDto.RuleName.Should().Be(HasNoCircularUsingsRuleMetadata.HasNoCircularUsings);
    }

    [Fact]
    public void ShouldParseNoUsingsRuleDefinition()
    {
      //GIVEN
      var depending = Any.String();

      //WHEN
      var from = Any.String();
      var to = Any.String();
      var ruleUnionDto = ParserRulePreface.Then(ParseNamespaceBasedRule.Complement)
        .Parse(TestRuleFormats.FormatNoUsingsRule(depending, from, to));

      //THEN
      ruleUnionDto.Accept(new NoUsingsRuleComplementDtoAssertion(dto =>
      {
        dto.RuleName.Should().Be(HasNoUsingsRuleMetadata.RuleName);
        dto.ProjectAssemblyNamePattern.Should().Be(Pattern.WithoutExclusion(depending));
        dto.FromPattern.Should().Be(Pattern.WithoutExclusion(from));
        dto.ToPattern.Should().Be(Pattern.WithoutExclusion(to));
      }));
      ruleUnionDto.RuleName.Should().Be(HasNoUsingsRuleMetadata.RuleName);
    }


    [Fact]
    public void ShouldParseHasAttributesOnRuleDefinitionMultipleTimes()
    {
      //GIVEN
      var depending = Any.String();
      var classPattern = Any.String();
      var methodPattern = Any.String();

      //WHEN
      var ruleUnionDtos = ParserRulePreface.Then(ParseProjectScopedRule.Complement).Many().Parse(
        $"{depending} {HasAttributesOnRuleMetadata.HasAttributesOn} {classPattern}:{methodPattern}{Environment.NewLine}" +
        $"{depending} {HasAttributesOnRuleMetadata.HasAttributesOn} {classPattern}:{methodPattern}{Environment.NewLine}"
      ).ToList();

      //THEN
      ruleUnionDtos.Count.Should().Be(2);
      var rule1Dto = ruleUnionDtos.First();
      rule1Dto.Accept(new HasAttributesOnRuleComplementAssertion(dto =>
      {
        dto.RuleName.Should().Be(HasAttributesOnRuleMetadata.HasAttributesOn);
        dto.ClassNameInclusionPattern.Description().Should().Be(classPattern);
        dto.MethodNameInclusionPattern.Description().Should().Be(methodPattern);
        dto.ProjectAssemblyNamePattern.Description().Should().Be(depending);
      }));
      rule1Dto.RuleName.Should().Be(HasAttributesOnRuleMetadata.HasAttributesOn);
    }

    [Fact]
    public void ShouldParseHasTargetFrameworkDefinitionMultipleTimes()
    {
      //GIVEN
      var depending = Any.String();
      var frameworkName = Any.String();

      //WHEN
      var ruleUnionDtos = ParserRulePreface.Then(ParseProjectScopedRule.Complement).Many().Parse(
        $"{depending} {HasTargetFrameworkRuleMetadata.HasTargetFramework} {frameworkName}{Environment.NewLine}" +
        $"{depending} {HasTargetFrameworkRuleMetadata.HasTargetFramework} {frameworkName}{Environment.NewLine}"
      ).ToList();

      //THEN
      ruleUnionDtos.Count.Should().Be(2);
      var rule1Dto = ruleUnionDtos.First();
      rule1Dto.Accept(new HasTargetFrameworkAssertion(dto =>
      {
        dto.RuleName.Should().Be(HasTargetFrameworkRuleMetadata.HasTargetFramework);
        dto.ProjectAssemblyNamePattern.Description().Should().Be(depending);
        dto.TargetFramework.Should().Be(frameworkName);
      }));
      rule1Dto.RuleName.Should().Be(HasTargetFrameworkRuleMetadata.HasTargetFramework);
    }
  }
}