using System.Collections.Generic;
using NScan.Domain.DependencyPathBasedRules;

namespace NScan.Domain.Root
{
  public interface IReportFragmentsFormat
  {
    string ApplyToPath(IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath);
    string ApplyToCycles(IReadOnlyList<IReadOnlyList<string>> cycles);
  }
}