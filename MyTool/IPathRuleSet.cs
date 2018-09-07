using System.Collections.Generic;
using MyTool.App;
using MyTool.CompositionRoot;

namespace MyTool
{
  public interface IPathRuleSet
  {
    void AddDirectIndependentOfProjectRule(ProjectId depending, ProjectId dependent);
    void Check(IPathCache path, IAnalysisReportInProgress report);
  }
}