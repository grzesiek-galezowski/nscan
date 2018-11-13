namespace TddXt.NScan.Domain
{
  public interface IPathCache
  {
    void BuildStartingFrom(params IDotNetProject[] rootProjects);
    void Check(IDependencyRule rule, IAnalysisReportInProgress report);
  }
}