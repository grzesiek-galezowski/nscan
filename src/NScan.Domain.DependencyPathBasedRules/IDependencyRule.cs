using NScan.SharedKernel;

namespace NScan.Domain.DependencyPathBasedRules
{
  public interface IDependencyRule
  {
    void Check(IAnalysisReportInProgress report, IProjectDependencyPath dependencyPath);
  }
}