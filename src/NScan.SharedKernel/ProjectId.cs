using System.Collections.Generic;
using Value;

namespace NScan.SharedKernel
{
  public class ProjectId : ValueType<ProjectId>
  {
    private readonly string _absolutePath;

    public ProjectId(string absolutePath)
    {
      _absolutePath = absolutePath;
    }

    public override string ToString()
    {
      return _absolutePath;
    }

    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
      yield return _absolutePath;
    }
  }
}
