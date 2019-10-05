using System;
using System.Collections.Generic;
using NScan.Lib;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos;
using Sprache;

namespace NScan.Adapter.ReadingRules
{
	//bug separate beginning parser from the other parsers
	public static class ParseProjectScopedRule
	{
		private static readonly Parser<string> TextUntilEol = Parse.AnyChar.Until(Parse.LineTerminator).Text().Token();

		public static Parser<RuleUnionDto> Complement(
			Pattern dependingPattern)
		{
			return HasAttributesOn(dependingPattern)
					.Or(HasTargetFramework(dependingPattern));
		}

		private static Parser<string> TextUntil(char c)
		{
			return Parse.AnyChar.Until(Parse.Char(c)).Text().Token();
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