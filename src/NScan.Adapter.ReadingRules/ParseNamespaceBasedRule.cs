using System.Collections.Generic;
using NScan.Lib;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using Sprache;

namespace NScan.Adapter.ReadingRules
{
  public static class ParseNamespaceBasedRule
  {
	  private static readonly Parser<IEnumerable<char>> OptionalSpacesUntilEol = Parse.WhiteSpace.Until(Parse.LineTerminator);

    public static Parser<NamespaceBasedRuleUnionDto> Complement(
      Pattern dependingPattern)
    {
      return HasNoCircularUsingsRuleComplement(dependingPattern);
    }

    private static Parser<NamespaceBasedRuleUnionDto> HasNoCircularUsingsRuleComplement(Pattern dependingPattern)
    {
      return Parse.String(HasNoCircularUsingsRuleMetadata.HasNoCircularUsings).Then(_ => OptionalSpacesUntilEol).Return(
        NamespaceBasedRuleUnionDto.With(new NoCircularUsingsRuleComplementDto(dependingPattern)));
    }
  }

}