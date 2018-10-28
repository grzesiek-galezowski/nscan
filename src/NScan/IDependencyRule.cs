namespace TddXt.NScan
{
  public interface IDependencyRule
  {
    void Check(IAnalysisReportInProgress report, IProjectDependencyPath dependencyPath);
  }
}