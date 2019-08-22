using System.Collections.Generic;
using Value;

namespace NScan.SharedKernel.ReadingSolution.Ports
{
  public sealed class PackageReference : ValueType<PackageReference>
  {
    public string Name { get; }
    private readonly string _version;

    public PackageReference(string name, string version)
    {
      Name = name;
      _version = version;
    }

    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
      yield return Name;
      yield return _version;
    }

    public override string ToString()
    {
      return $"{Name}, Version {_version}";
    }
  }
}