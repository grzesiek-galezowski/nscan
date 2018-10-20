using System.Collections.Generic;
using TddXt.NScan.App;

namespace TddXt.NScan
{
  public interface IDependencyRule
  {
    void Check(IReadOnlyList<IReferencedProject> path, IAnalysisReportInProgress report);
  }
}