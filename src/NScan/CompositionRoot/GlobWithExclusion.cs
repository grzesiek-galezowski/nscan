using System;
using GlobExpressions;

namespace TddXt.NScan.CompositionRoot
{
  public sealed class GlobWithExclusion : IEquatable<GlobWithExclusion>
  {
    public bool Equals(GlobWithExclusion other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Equals(ReturnValue.Pattern, other.ReturnValue.Pattern);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((GlobWithExclusion) obj);
    }

    public override int GetHashCode()
    {
      return (ReturnValue != null ? ReturnValue.GetHashCode() : 0);
    }

    public static bool operator ==(GlobWithExclusion left, GlobWithExclusion right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(GlobWithExclusion left, GlobWithExclusion right)
    {
      return !Equals(left, right);
    }

    public GlobWithExclusion(Glob returnValue)
    {
      ReturnValue = returnValue;
      
    }

    public Glob ReturnValue { get; } //bug remove
  }
}