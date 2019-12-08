using System.Collections.Generic;
using NScan.DependencyPathBasedRules;

namespace NScan.Domain.Root
{
  public interface IDependencyPathReportFragmentsFormat
  {
    string ApplyToPath(IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath);
  }
}