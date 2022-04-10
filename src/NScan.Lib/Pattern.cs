using System;
using Core.Maybe;
using GlobExpressions;

namespace NScan.Lib;

public sealed record Pattern
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
    return new Pattern(
      pattern ?? throw new ArgumentNullException(nameof(pattern)), 
      Maybe<string>.Nothing);
  }

  public static Pattern WithExclusion(string inclusionPattern, string exclusionPattern)
  {
    return new Pattern(
      inclusionPattern ?? throw new ArgumentNullException(nameof(inclusionPattern)),
      exclusionPattern.Just()
    );
  }

  public string Text()
  {
    return _exclusionPattern.Select(exclusionPattern => _inclusionPattern + " except " + exclusionPattern)
      .OrElse(() => _inclusionPattern);
  }

  public bool IsMatchedBy(string expected)
  {
    return
      Glob.IsMatch(expected, _inclusionPattern)
      && _exclusionPattern.Select(
          exclusion => !Glob.IsMatch(expected, exclusion))
        .OrElse(() => true);
  }
}
