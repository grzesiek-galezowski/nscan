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
      var either = ParseRule.FromLine()
        .Parse($"{depending} {RuleNames.IndependentOf} {dependencyType}:{dependency}{NewLine}");

      //THEN
      either.Left.DependingPattern.Should().Be(Pattern.WithoutExclusion(depending));
      either.Left.DependencyType.Should().Be(dependencyType);
      either.Left.DependencyPattern.Pattern.Should().Be(dependency);
      either.Left.RuleName.Should().Be(RuleNames.IndependentOf);
      either.RuleName.Should().Be(RuleNames.IndependentOf);
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
      var either = ParseRule.FromLine()
        .Parse($"{depending} except {dependingException} {RuleNames.IndependentOf} {dependencyType}:{dependency}{NewLine}");

      //THEN
      either.Left.DependingPattern.Should().Be(Pattern.WithExclusion(depending, dependingException));
      either.Left.DependencyType.Should().Be(dependencyType);
      either.Left.DependencyPattern.Pattern.Should().Be(dependency);
      either.Left.RuleName.Should().Be(RuleNames.IndependentOf);
      either.RuleName.Should().Be(RuleNames.IndependentOf);
    }

    [Fact]
    public void ShouldParseDefaultRuleSyntaxWithMoreThanOneSpace()
    {
      //GIVEN
      var depending = Any.String();
      var dependencyType = Any.String();
      var dependency = Any.String();

      //WHEN
      var either = ParseRule.FromLine()
        .Parse($"{depending}  {RuleNames.IndependentOf}  {dependencyType}:{dependency}{NewLine}");

      //THEN
      either.Left.DependencyType.Should().Be(dependencyType);
      either.Left.DependencyPattern.Pattern.Should().Be(dependency);
      either.Left.DependingPattern.Should().Be(Pattern.WithoutExclusion(depending));
      either.Left.RuleName.Should().Be(RuleNames.IndependentOf);
      either.RuleName.Should().Be(RuleNames.IndependentOf);
    }

    [Fact]
    public void ShouldParseNamespacesIntactRuleDefinition()
    {
      //GIVEN
      var depending = Any.String();

      //WHEN
      var either = ParseRule.FromLine()
        .Parse($"{depending}  {RuleNames.HasCorrectNamespaces}");

      //THEN
      either.Left.Should().BeNull(); //bug maybe!
      either.Right.Should().NotBeNull();
      either.Right.RuleName.Should().Be(RuleNames.HasCorrectNamespaces);
      either.Right.ProjectAssemblyNamePattern.Should()
        .Be(Pattern.WithoutExclusion(depending));
      either.RuleName.Should().Be(RuleNames.HasCorrectNamespaces);
    }
  }
}