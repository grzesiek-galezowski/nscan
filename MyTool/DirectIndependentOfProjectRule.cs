using System.Collections.Generic;
using MyTool.App;

namespace MyTool
{
  public class DirectIndependentOfProjectRule : IDependencyRule
  {
    private readonly ProjectId _dependingId;
    private readonly ProjectId _dependencyId;

    public DirectIndependentOfProjectRule(ProjectId dependingId, ProjectId dependencyId)
    {
      _dependingId = dependingId;
      _dependencyId = dependencyId;
    }

    public void Check(IReadOnlyList<IReferencedProject> path, IAnalysisReportInProgress report)
    {
    }
  }
}