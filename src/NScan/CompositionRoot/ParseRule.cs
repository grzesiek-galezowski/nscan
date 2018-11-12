using System.Collections.Generic;
using GlobExpressions;
using Sprache;
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
        from complementDto in Complement()
        select new RuleDto
        {
          DependingPattern = DependingPattern(depending, optionalException),
          IndependentRuleComplement = complementDto.IndependentRuleComplement
        };
    }

    private static Parser<ComplementDto> Complement()
    {
      return IndependentOfKeyword
        .Then(_ => from dependencyType in AnyChar.Until(Char(':')).Text().Token()
          from dependency in TextUntilEol
          select new ComplementDto
          {
            IndependentRuleComplement = new IndependentRuleComplementDto
            {
              RuleName = "independentOf",
              DependencyPattern = new Glob(dependency),
              DependencyType = dependencyType
            }
          });
    }

    private static Pattern DependingPattern(string depending, IOption<string> optionalException)
    {
      return optionalException.IsDefined ? Pattern.WithExclusion(depending, optionalException.Get()) : Pattern.WithoutExclusion(depending);
    }
  }

  internal class ComplementDto
  {
    public IndependentRuleComplementDto IndependentRuleComplement { get; set; }
  }

  public class IndependentRuleComplementDto
  {
    public Glob DependencyPattern { get; set; }
    public string DependencyType { get; set; }
    public string RuleName { get; set; }
  }
}