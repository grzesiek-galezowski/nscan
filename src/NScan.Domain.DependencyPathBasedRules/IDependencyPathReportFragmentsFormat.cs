using System.Collections.Generic;

namespace NScan.DependencyPathBasedRules
{
  public interface IDependencyPathReportFragmentsFormat
  {
    string ApplyToPath(IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath);
  }
}