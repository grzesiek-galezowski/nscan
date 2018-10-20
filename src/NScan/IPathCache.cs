using TddXt.NScan.App;

namespace TddXt.NScan
{
  public interface IPathCache
  {
    void BuildStartingFrom(params IDotNetProject[] rootProjects);
    void Check(IDependencyRule rule, IAnalysisReportInProgress report);
  }
}