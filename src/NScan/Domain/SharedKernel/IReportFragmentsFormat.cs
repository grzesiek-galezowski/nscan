using System.Collections.Generic;
using TddXt.NScan.Domain.DependencyPathBasedRules;

namespace TddXt.NScan.Domain.SharedKernel
{
  public interface IReportFragmentsFormat
  {
    string ApplyToPath(IReadOnlyList<IDependencyPathBasedRuleTarget> violationPath);
    string ApplyToCycles(IReadOnlyList<IReadOnlyList<string>> cycles);
  }
}