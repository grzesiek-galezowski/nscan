using System.Collections.Generic;
using MyTool.App;

namespace MyTool
{
  public interface IDependencyRule
  {
    void Check(IReadOnlyList<IReferencedProject> path, IAnalysisReportInProgress report);
  }
}