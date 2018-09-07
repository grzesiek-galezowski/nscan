using System.Collections.Generic;
using MyTool.App;

namespace MyTool.CompositionRoot
{
  public class PathRuleSet : IPathRuleSet
  {
    public void Add(IDependencyRule rule)
    {
      throw new System.NotImplementedException();
    }

    public void Check(IPathCache path, IAnalysisReportInProgress report)
    {
      throw new System.NotImplementedException();
    }
  }
}