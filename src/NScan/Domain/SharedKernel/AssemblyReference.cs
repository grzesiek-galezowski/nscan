using System.Collections.Generic;
using Value;

namespace TddXt.NScan.Domain.SharedKernel
{
  public sealed class AssemblyReference : ValueType<AssemblyReference>
  {
    private readonly string _hintPath;

    public AssemblyReference(string assemblyReferenceName, string hintPath)
    {
      Name = assemblyReferenceName;
      _hintPath = hintPath;
    }

    public string Name { get; }

    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
      yield return _hintPath;
      yield return Name;
    }
  }
}