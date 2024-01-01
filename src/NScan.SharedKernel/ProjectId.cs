using System;

namespace NScan.SharedKernel;

public sealed record ProjectId(string AbsolutePath)
{
  public override string ToString()
  {
    return AbsolutePath;
  }

  public int CompareTo(ProjectId? other)
  {
    if (ReferenceEquals(this, other))
    {
      return 0;
    }

    if (ReferenceEquals(null, other))
    {
      return 1;
    }

    return string.Compare(AbsolutePath, other.AbsolutePath, StringComparison.Ordinal);
  }
}
