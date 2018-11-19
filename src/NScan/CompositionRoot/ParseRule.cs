using System.Collections.Generic;
using GlobExpressions;
using Sprache;
using TddXt.NScan.Domain;
using TddXt.NScan.RuleInputData;
using static Sprache.Parse;

namespace TddXt.NScan.CompositionRoot
{
  public static class ParseRule
  {
    private static readonly Parser<IEnumerable<char>> Spaces = WhiteSpace.AtLeastOnce();
    private static readonly Parser<string> TextUntilWhitespace = AnyChar.Until(Spaces).Text();
    private static readonly Parser<IEnumerable<char>> ExceptKeyword = String("except");
    private static readonly Parser<string> TextUntilEol = AnyChar.Until(LineEnd).Text().Token();
    private static readonly Parser<IEnumerable<char>> IndependentOfKeyword = String(RuleNames.IndependentOf);

    public static Parser<RuleDto> FromLine()
    {
      return from depending in TextUntilWhitespace
        from optionalException in ExceptKeyword.Token().Then(_ => TextUntilWhitespace).Optional()
        from either in Complement(DependingPattern(depending, optionalException))
        select new RuleDto
        {
          Either = either,
          DependingPattern = DependingPattern(depending, optionalException), //bug remove
          IndependentRuleComplement = either.Left, //bug maybe //bug remove
          CorrectNamespacesRuleComplement = either.Right, //bug maybe //bug remove
          RuleName = either.Left?.RuleName ?? either.Right.RuleName //bug remove
        };
    }

    private static Parser<Either<IndependentRuleComplementDto, CorrectNamespacesRuleComplementDto>> Complement(
      Pattern dependingPattern)
    {
      return IndependentOfRuleComplement(dependingPattern)
        .Or(HasCorrectNamespacesRuleComplement(dependingPattern));
    }

    private static Parser<Either<IndependentRuleComplementDto, CorrectNamespacesRuleComplementDto>>
      HasCorrectNamespacesRuleComplement(Pattern dependingPattern)
    {
      return String(RuleNames.HasCorrectNamespaces).Return(
        new Either<IndependentRuleComplementDto, CorrectNamespacesRuleComplementDto>
      {
        Right = new CorrectNamespacesRuleComplementDto()
        {
          ProjectAssemblyNamePattern = dependingPattern,
        },
        RuleName = RuleNames.HasCorrectNamespaces
      });
    }

    private static Parser<Either<IndependentRuleComplementDto, CorrectNamespacesRuleComplementDto>>
      IndependentOfRuleComplement(Pattern dependingPattern)
    {

      return IndependentOfKeyword
        .Then(_ => from dependencyType in AnyChar.Until(Char(':')).Text().Token()
          from dependency in TextUntilEol
          select new Either<IndependentRuleComplementDto, CorrectNamespacesRuleComplementDto>
          {
            Left = new IndependentRuleComplementDto
            {
              DependingPattern = dependingPattern,
              DependencyPattern = new Glob(dependency),
              DependencyType = dependencyType,
              RuleName = RuleNames.IndependentOf,
            },
            RuleName = RuleNames.HasCorrectNamespaces
          });
    }

    private static Pattern DependingPattern(string depending, IOption<string> optionalException)
    {
      return optionalException.IsDefined ? Pattern.WithExclusion(depending, optionalException.Get()) : Pattern.WithoutExclusion(depending);
    }
  }

  public enum RuleTypes
  {
    PathRule, 
    ProjectScopedRule
  }
}