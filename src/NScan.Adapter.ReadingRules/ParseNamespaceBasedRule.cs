using System.Collections.Generic;
using NScan.Lib;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using Sprache;

namespace NScan.Adapter.ReadingRules
{
  public static class ParseNamespaceBasedRule
  {
	  private static readonly Parser<IEnumerable<char>> OptionalSpacesUntilEol = Parse.WhiteSpace.Until(Parse.LineTerminator);
    private static readonly Parser<string> TextUntilEol = Parse.AnyChar.Until(Parse.LineTerminator).Text().Token();
    private static readonly Parser<IEnumerable<char>> Spaces = Parse.WhiteSpace.AtLeastOnce();
    private static readonly Parser<string> TextUntilWhitespace = Parse.AnyChar.Until(Spaces).Text();

    public static Parser<NamespaceBasedRuleUnionDto> Complement(Pattern dependingPattern)
    {
      return HasNoCircularUsingsRuleComplement(dependingPattern)
        .Or(HasNoUsingsRuleComplement(dependingPattern));
    }

    private static Parser<NamespaceBasedRuleUnionDto> HasNoUsingsRuleComplement(Pattern dependingPattern)
    {
      return Parse.String(HasNoUsingsRuleMetadata.RuleName)
        .Then(_ => Spaces)
        .Then(_ => 
          from fromKeyWord in Parse.String("from").Then(_ => Spaces)
          from fromPattern in TextUntilWhitespace
          from toKeyWord in Parse.String("to").Then(_ => Spaces)
          from toPattern in TextUntilEol
          select NamespaceBasedRuleUnionDto.With(
            new NoUsingsRuleComplementDto(
              dependingPattern, 
              Pattern.WithoutExclusion(fromPattern), 
              Pattern.WithoutExclusion(toPattern))));
    }

    private static Parser<NamespaceBasedRuleUnionDto> HasNoCircularUsingsRuleComplement(Pattern dependingPattern)
    {
      return Parse.String(HasNoCircularUsingsRuleMetadata.HasNoCircularUsings).Then(_ => OptionalSpacesUntilEol).Return(
        NamespaceBasedRuleUnionDto.With(new NoCircularUsingsRuleComplementDto(dependingPattern)));
    }
  }

}