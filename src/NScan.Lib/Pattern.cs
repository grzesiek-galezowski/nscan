using System;
using System.Collections.Generic;
using Functional.Maybe;
using Functional.Maybe.Just;
using GlobExpressions;
using Value;

namespace NScan.Lib
{
  public sealed class Pattern : ValueType<Pattern>
  {
    private readonly Maybe<string> _exclusionPattern;
    private readonly string _inclusionPattern;

    public Pattern(string inclusionPattern, Maybe<string> exclusionPattern)
    {
      _inclusionPattern = inclusionPattern;
      _exclusionPattern = exclusionPattern;
    }

    public static Pattern WithoutExclusion(string pattern)
    {
      return new(
        pattern ?? throw new ArgumentNullException(nameof(pattern)), 
        Maybe<string>.Nothing);
    }

    public static Pattern WithExclusion(string inclusionPattern, string exclusionPattern)
    {
      return new(
        inclusionPattern ?? throw new ArgumentNullException(nameof(inclusionPattern)),
        exclusionPattern.Just()
        );
    }

    public string Description()
    {
      return _exclusionPattern.Select(exclusionPattern => _inclusionPattern + " except " + exclusionPattern)
        .OrElse(() => _inclusionPattern);
    }

    public bool IsMatch(string expected)
    {
      return
        Glob.IsMatch(expected, _inclusionPattern)
      && _exclusionPattern.Select(
          exclusion => !Glob.IsMatch(expected, exclusion))
          .OrElse(() => true);
    }

    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
      yield return _exclusionPattern;
      yield return _inclusionPattern;
    }
  }
}
