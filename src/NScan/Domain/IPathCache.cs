namespace TddXt.NScan.Domain
{
  public interface IPathCache
  {
    void BuildStartingFrom(params IReferencingProject[] rootProjects);
    void Check(IDependencyRule rule, IAnalysisReportInProgress report);
  }
}