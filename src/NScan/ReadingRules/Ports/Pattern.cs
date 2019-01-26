using System;
using Functional.Maybe;
using Functional.Maybe.Just;
using GlobExpressions;

namespace TddXt.NScan.ReadingRules.Ports
{
  public sealed class Pattern : IEquatable<Pattern>
  {
    private readonly string _inclusionPattern;
    private readonly Maybe<string> _exclusionPattern;

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

    public Pattern(string inclusionPattern, Maybe<string> exclusionPattern)
    {
      _inclusionPattern = inclusionPattern;
      _exclusionPattern = exclusionPattern;
    }


    public string Description()
    {
      return _exclusionPattern.Select(exclusionPattern => _inclusionPattern + " except " + exclusionPattern)
        .OrElse(() => _inclusionPattern);
    }

    public bool IsMatch(string assemblyName)
    {
      return
        Glob.IsMatch(assemblyName, _inclusionPattern)
      && _exclusionPattern.Select(
          exclusion => !Glob.IsMatch(assemblyName, exclusion))
          .OrElse(() => true);
    }

    public bool Equals(Pattern other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return string.Equals(_inclusionPattern, other._inclusionPattern) && _exclusionPattern.Equals(other._exclusionPattern);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      return obj is Pattern other && Equals(other);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (_inclusionPattern.GetHashCode() * 397) ^ _exclusionPattern.GetHashCode();
      }
    }

    public static bool operator ==(Pattern left, Pattern right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(Pattern left, Pattern right)
    {
      return !Equals(left, right);
    }


  }
}