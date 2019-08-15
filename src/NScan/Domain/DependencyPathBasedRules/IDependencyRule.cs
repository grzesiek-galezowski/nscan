using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.DependencyPathBasedRules
{
  public interface IDependencyRule
  {
    void Check(IAnalysisReportInProgress report, IProjectDependencyPath dependencyPath);
  }
}