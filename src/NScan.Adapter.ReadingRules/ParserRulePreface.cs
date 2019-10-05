using System;
using System.Collections.Generic;
using NScan.Lib;
using Sprache;

namespace NScan.Adapter.ReadingRules
{
	public static class ParserRulePreface
	{
		private static readonly Parser<IEnumerable<char>> Spaces = Parse.WhiteSpace.AtLeastOnce();
		private static readonly Parser<string> TextUntilWhitespace = Parse.AnyChar.Until(Spaces).Text();
		private static readonly Parser<IEnumerable<char>> ExceptKeyword = Parse.String("except");

		public static Parser<T> Then<T>(Func<Pattern, Parser<T>> ruleComplementFactory)
		{
			return from depending in TextUntilWhitespace
				from optionalException in ExceptKeyword.Token().Then(_ => TextUntilWhitespace).Optional()
				from ruleUnion in ruleComplementFactory(DependingPattern(depending, optionalException))
				select ruleUnion;
		}

		private static Pattern DependingPattern(string depending, IOption<string> optionalException)
		{
			return optionalException.IsDefined ? Pattern.WithExclusion(depending, optionalException.Get()) : Pattern.WithoutExclusion(depending);
		}
	}
}