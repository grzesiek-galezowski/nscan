using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRules
{
  public interface IDependencyRule
  {
    void Check(IAnalysisReportInProgress report, IProjectDependencyPath dependencyPath);
  }
}