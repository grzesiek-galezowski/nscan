using System;

namespace TddXt.NScan.CompositionRoot
{
  public sealed class PackageReference : IEquatable<PackageReference>
  {
    public bool Equals(PackageReference other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return string.Equals(_name, other._name) && string.Equals(_version, other._version);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((PackageReference) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (_name.GetHashCode() * 397) ^ _version.GetHashCode();
      }
    }

    public static bool operator ==(PackageReference left, PackageReference right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(PackageReference left, PackageReference right)
    {
      return !Equals(left, right);
    }

    private readonly string _name;
    private readonly string _version;

    public PackageReference(string name, string version)
    {
      _name = name;
      _version = version;
    }
  }
}