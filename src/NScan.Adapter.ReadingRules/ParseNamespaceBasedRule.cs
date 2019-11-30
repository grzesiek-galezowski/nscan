using System.Collections.Generic;
using NScan.Lib;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos;
using Sprache;

namespace NScan.Adapter.ReadingRules
{
  public static class ParseNamespaceBasedRule
  {
	  private static readonly Parser<IEnumerable<char>> OptionalSpacesUntilEol = Parse.WhiteSpace.Until(Parse.LineTerminator);

    public static Parser<RuleUnionDto> Complement(
      Pattern dependingPattern)
    {
      return HasNoCircularUsingsRuleComplement(dependingPattern);
    }

    private static Parser<RuleUnionDto> HasNoCircularUsingsRuleComplement(Pattern dependingPattern)
    {
      return Parse.String(HasNoCircularUsingsRuleMetadata.HasNoCircularUsings).Then(_ => OptionalSpacesUntilEol).Return(
        RuleUnionDto.With(new NoCircularUsingsRuleComplementDto(dependingPattern)));
    }
  }

}