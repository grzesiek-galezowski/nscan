using System.Collections.Generic;
using System.Linq;
using LanguageExt;

namespace NScan.DependencyPathBasedRules;

public class DependencyPathReportFragmentsFormat : IDependencyPathReportFragmentsFormat
{
  public string ApplyToPath(Seq<IDependencyPathBasedRuleTarget> violationPath)
  {
    return violationPath.Skip(1).Aggregate(
      "[" + violationPath.First().ToString() + "]",
      (total, current) => $"{total}->[{current.ToString()}]");
  }
}
