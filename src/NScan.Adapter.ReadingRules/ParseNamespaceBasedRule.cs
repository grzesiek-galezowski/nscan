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
      return HasCorrectNamespacesRuleComplement(dependingPattern)
        .Or(HasNoCircularUsingsRuleComplement(dependingPattern));
    }

    private static Parser<RuleUnionDto> HasNoCircularUsingsRuleComplement(Pattern dependingPattern)
    {
      return Parse.String(HasNoCircularUsingsRuleMetadata.HasNoCircularUsings).Then(_ => OptionalSpacesUntilEol).Return(
        RuleUnionDto.With(new NoCircularUsingsRuleComplementDto(dependingPattern)));
    }

    private static Parser<RuleUnionDto>
      HasCorrectNamespacesRuleComplement(Pattern dependingPattern)
    {
      return Parse.String(HasCorrectNamespacesRuleMetadata.HasCorrectNamespaces).Then(_ => OptionalSpacesUntilEol).Return(
        RuleUnionDto.With(new CorrectNamespacesRuleComplementDto(dependingPattern)));
    }
  }

}