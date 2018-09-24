using System.Collections.Generic;
using NScanRoot.App;

namespace NScanRoot
{
  public interface IDependencyRule
  {
    void Check(IReadOnlyList<IReferencedProject> path, IAnalysisReportInProgress report);
  }
}