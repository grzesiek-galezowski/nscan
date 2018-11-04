using System.Collections.Generic;
using GlobExpressions;
using Sprache;
using static Sprache.Parse;

namespace TddXt.NScan.CompositionRoot
{
  public static class ParseRule
  {
    private static readonly Parser<IEnumerable<char>> Spaces = WhiteSpace.AtLeastOnce();

    public static Parser<RuleDto> FromLine()
    {
      return from depending in AnyChar.Until(Spaces).Text()
        from optionalException in String("except").Then(_ => Spaces).Then(_ => AnyChar.Until(Spaces)).Optional()
        from ruleName in AnyChar.Until(Spaces).Text()
        from dependencyType in AnyChar.Until(Char(':')).Text()
        from dependency in AnyChar.Until(LineEnd).Text()
        select new RuleDto
        {
          DependingPattern = new Glob(depending),
          RuleName = ruleName,
          DependencyPattern = new Glob(dependency),
          DependencyType = dependencyType
        };
    }
  }
}