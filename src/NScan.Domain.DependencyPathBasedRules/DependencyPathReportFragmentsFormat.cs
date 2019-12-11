using System.Collections.Generic;
using System.Linq;

namespace NScan.DependencyPathBasedRules
{
  public class DependencyPathReportFragmentsFormat : IDependencyPathReportFragmentsFormat
  {
    public string ApplyToPath(IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath)
    {
      return violationPath.Skip(1).Aggregate(
        "[" + violationPath.First().ToString() + "]",
        (total, current) => $"{total}->[{current.ToString()}]");
    }
  }
}