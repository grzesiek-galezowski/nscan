using System.Collections.Generic;
using GlobExpressions;
using NScan.Lib;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using Sprache;

namespace NScan.Adapter.ReadingRules
{
  public static class ParseDependencyPathBasedRule
  {
	  private static readonly Parser<string> TextUntilEol = Parse.AnyChar.Until(Parse.LineTerminator).Text().Token();
    private static readonly Parser<IEnumerable<char>> IndependentOfKeyword = Parse.String(IndependentRuleMetadata.IndependentOf);

    public static Parser<DependencyPathBasedRuleUnionDto> Complement(
      Pattern dependingPattern)
    {
      return IndependentOfRuleComplement(dependingPattern);
    }

    private static Parser<DependencyPathBasedRuleUnionDto>
      IndependentOfRuleComplement(Pattern dependingPattern)
    {

      return IndependentOfKeyword
        .Then(_ => 
          from dependencyType in TextUntil(':')
          from dependency in TextUntilEol
          select DependencyPathBasedRuleUnionDto.With(
            new IndependentRuleComplementDto(dependencyType, 
              dependingPattern, new Glob(dependency))));
    }

    private static Parser<string> TextUntil(char c)
    {
      return Parse.AnyChar.Until(Parse.Char(c)).Text().Token();
    }
  }
}