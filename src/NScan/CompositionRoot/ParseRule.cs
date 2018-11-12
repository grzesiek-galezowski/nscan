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
    private static readonly Parser<string> TextUntilEol = AnyChar.Until(LineEnd).Text();

    public static Parser<RuleDto> FromLine()
    {
      return from depending in TextUntilWhitespace
        from optionalException in ExceptKeyword.Then(_ => Spaces).Then(_ => TextUntilWhitespace).Optional()
        from ruleName in String(RuleNames.IndependentOf).Then(_ => Spaces).Return(RuleNames.IndependentOf)
        from dependencyType in AnyChar.Until(Char(':')).Text()
        from dependency in TextUntilEol
        select new RuleDto
        {
          DependingPattern = DependingPattern(depending, optionalException),
          RuleName = ruleName,
          DependencyPattern = new Glob(dependency),
          DependencyType = dependencyType
        };
    }

    private static Pattern DependingPattern(string depending, IOption<string> optionalException)
    {
      return optionalException.IsDefined ? Pattern.WithExclusion(depending, optionalException.Get()) : Pattern.WithoutExclusion(depending);
    }
  }
}