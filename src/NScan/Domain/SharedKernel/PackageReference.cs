using System;

namespace TddXt.NScan.Domain.SharedKernel
{
  public sealed class PackageReference : IEquatable<PackageReference>
  {
    private readonly string _version;

    public PackageReference(string name, string version)
    {
      Name = name;
      _version = version;
    }

    public string Name { get; }

    public bool Equals(PackageReference other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return string.Equals(Name, other.Name) && string.Equals(_version, other._version);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((PackageReference) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return (Name.GetHashCode() * 397) ^ _version.GetHashCode();
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

    public override string ToString()
    {
      return $"{Name}, Version {_version}";
    }
  }
}