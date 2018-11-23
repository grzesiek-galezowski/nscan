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

    public static Parser<RuleUnionDto> FromLine()
    {
      return from depending in TextUntilWhitespace
        from optionalException in ExceptKeyword.Token().Then(_ => TextUntilWhitespace).Optional()
        from either in Complement(DependingPattern(depending, optionalException))
        select either;
    }

    private static Parser<RuleUnionDto> Complement(
      Pattern dependingPattern)
    {
      return IndependentOfRuleComplement(dependingPattern)
        .Or(HasCorrectNamespacesRuleComplement(dependingPattern));
    }

    private static Parser<RuleUnionDto>
      HasCorrectNamespacesRuleComplement(Pattern dependingPattern)
    {
      return String(RuleNames.HasCorrectNamespaces).Return(
        RuleUnionDto.With(new CorrectNamespacesRuleComplementDto()
        {
          ProjectAssemblyNamePattern = dependingPattern,
        }));
    }

    private static Parser<RuleUnionDto>
      IndependentOfRuleComplement(Pattern dependingPattern)
    {

      return IndependentOfKeyword
        .Then(_ => from dependencyType in AnyChar.Until(Char(':')).Text().Token()
          from dependency in TextUntilEol
          select RuleUnionDto.With(new IndependentRuleComplementDto
            {
              DependingPattern = dependingPattern,
              DependencyPattern = new Glob(dependency),
              DependencyType = dependencyType,
              RuleName = RuleNames.IndependentOf,
            }));
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