using System.Collections.Generic;
using System.Linq;

namespace TddXt.NScan.Domain
{
  public class PlainProjectPathFormat : IProjectPathFormat
  {
    public string ApplyTo(IReadOnlyList<IReferencedProject> violationPath)
    {
      return violationPath.Skip(1).Aggregate(
        "[" + violationPath.First().ToString() + "]",
        (total, current) => total + "->" + "[" + current.ToString() + "]");
    }
  }
}