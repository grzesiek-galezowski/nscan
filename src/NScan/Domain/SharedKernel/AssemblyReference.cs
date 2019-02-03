using System;

namespace TddXt.NScan.Domain.SharedKernel
{
  public sealed class AssemblyReference : IEquatable<AssemblyReference>
  {
    public string Name { get; }
    private readonly string _hintPath;

    public AssemblyReference(string assemblyReferenceName, string hintPath)
    {
      Name = assemblyReferenceName;
      _hintPath = hintPath;
    }

    public bool Equals(AssemblyReference other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return string.Equals((string) Name, (string) other.Name) && string.Equals((string) _hintPath, (string) other._hintPath);
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != GetType()) return false;
      return Equals((AssemblyReference) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (_hintPath != null ? _hintPath.GetHashCode() : 0);
      }
    }

    public static bool operator ==(AssemblyReference left, AssemblyReference right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(AssemblyReference left, AssemblyReference right)
    {
      return !Equals(left, right);
    }
  }
}