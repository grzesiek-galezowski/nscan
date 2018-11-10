using System;
using GlobExpressions;
using TddXt.NScan.ForFun;

namespace TddXt.NScan.CompositionRoot
{
  public sealed class Pattern : IEquatable<Pattern>
  {
    private readonly string _inclusionGlobString;
    private readonly Maybe<string> _exclusionGlob;

    public static Pattern WithoutExclusion(string depending)
    {
      return new Pattern(
        depending ?? throw new ArgumentNullException(nameof(depending)), 
        Maybe.Nothing<string>());
    }

    public static Pattern WithExclusion(string depending, string dependingException)
    {
      return new Pattern(
        depending ?? throw new ArgumentNullException(nameof(depending)),
        Maybe.Just(dependingException)
        ); //bug
    }

    public Pattern(string inclusionGlobString, Maybe<string> exclusionGlobString)
    {
      _inclusionGlobString = inclusionGlobString;
      _exclusionGlob = exclusionGlobString;
    }


    public string Description()
    {
      return _inclusionGlobString;
    }

    public bool IsMatch(string assemblyName)
    {
      return
        Glob.IsMatch(assemblyName, _inclusionGlobString);
      //&& _exclusionGlob.Select(exclusion => !Glob.IsMatch(assemblyName, exclusion)).Otherwise(() => true);
    }

    public bool Equals(Pattern other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return string.Equals(_inclusionGlobString, other._inclusionGlobString) && _exclusionGlob.Equals(other._exclusionGlob);
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
        return (_inclusionGlobString.GetHashCode() * 397) ^ _exclusionGlob.GetHashCode();
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