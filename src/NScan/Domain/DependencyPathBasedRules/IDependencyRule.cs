using NScan.SharedKernel.SharedKernel;

namespace TddXt.NScan.Domain.DependencyPathBasedRules
{
  public interface IDependencyRule
  {
    void Check(IAnalysisReportInProgress report, IProjectDependencyPath dependencyPath);
  }
}