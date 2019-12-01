using System;
using System.Collections.Generic;
using NScan.Lib;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using Sprache;

namespace NScan.Adapter.ReadingRules
{
	public static class ParseProjectScopedRule
	{
		private static readonly Parser<string> TextUntilEol = Parse.AnyChar.Until(Parse.LineTerminator).Text().Token();
    private static readonly Parser<IEnumerable<char>> OptionalSpacesUntilEol = Parse.WhiteSpace.Until(Parse.LineTerminator);

		public static Parser<RuleUnionDto> Complement(
			Pattern dependingPattern)
		{
			return HasCorrectNamespacesRuleComplement(dependingPattern)
        .Or(HasAttributesOn(dependingPattern))
					.Or(HasTargetFramework(dependingPattern));
		}

		private static Parser<string> TextUntil(char c)
		{
			return Parse.AnyChar.Until(Parse.Char(c)).Text().Token();
		}

    private static Parser<RuleUnionDto>
      HasCorrectNamespacesRuleComplement(Pattern dependingPattern)
    {
      return Parse.String(HasCorrectNamespacesRuleMetadata.HasCorrectNamespaces)
        .Then(_ => OptionalSpacesUntilEol)
        .Return(RuleUnionDto.With(new CorrectNamespacesRuleComplementDto(dependingPattern)));
    }


		private static Parser<RuleUnionDto> HasAttributesOn(Pattern dependingPattern)
		{
			return Parse.String(HasAttributesOnRuleMetadata.HasAttributesOn)
				.Then(_ =>
					from classPattern in TextUntil(':')
					from methodPattern in TextUntilEol
					select RuleUnionDto.With(
						new HasAttributesOnRuleComplementDto(
							dependingPattern, 
							Pattern.WithoutExclusion(classPattern),
							Pattern.WithoutExclusion(methodPattern))));
		}

		private static Parser<RuleUnionDto> HasTargetFramework(Pattern dependingPattern)
		{
			return Parse.String(HasTargetFrameworkRuleMetadata.HasTargetFramework)
				.Then(_ =>
					from targetFramework in TextUntilEol
					select RuleUnionDto.With(
						new HasTargetFrameworkRuleComplementDto(
							dependingPattern, targetFramework)));
		}
	}
}