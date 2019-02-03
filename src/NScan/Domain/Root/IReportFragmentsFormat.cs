using System.Collections.Generic;
using TddXt.NScan.Domain.DependencyPathBasedRules;

namespace TddXt.NScan.Domain.Root
{
  public interface IReportFragmentsFormat
  {
    string ApplyToPath(IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath);
    string ApplyToCycles(IReadOnlyList<IReadOnlyList<string>> cycles);
  }
}