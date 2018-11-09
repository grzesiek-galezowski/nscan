using System;
using GlobExpressions;

namespace TddXt.NScan.CompositionRoot
{
  public sealed class Pattern : IEquatable<Pattern>
  {
    public bool Equals(Pattern other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Equals(InnerGlob.Pattern, other.InnerGlob.Pattern);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((Pattern) obj);
    }

    public override int GetHashCode()
    {
      return (InnerGlob != null ? InnerGlob.Pattern.GetHashCode() : 0);
    }

    public static bool operator ==(Pattern left, Pattern right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(Pattern left, Pattern right)
    {
      return !Equals(left, right);
    }

    public Pattern(string innerGlobPattern)
    {
      InnerGlob = new Glob(innerGlobPattern);
    }

    private Glob InnerGlob { get; } //bug remove

    public string Description()
    {
      return InnerGlob.Pattern;
    }

    public bool IsMatch(string assemblyName)
    {
      return InnerGlob.IsMatch(assemblyName);
    }

    public static Pattern WithoutExclusion(string depending)
    {
      return new Pattern(depending ?? throw new ArgumentNullException(nameof(depending)));
    }

    public static Pattern WithExclusion(string depending, string dependingException)
    {
      return Pattern.WithoutExclusion(depending); //bug
    }
  }
}