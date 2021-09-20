using System.Collections.Generic;
using Value;

namespace NScan.SharedKernel
{
  public sealed record ProjectId(string AbsolutePath)
  {
    public override string ToString()
    {
      return AbsolutePath;
    }
  }
}
