using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.DependencyPathBasedRules
{
  public interface IDependencyRule
  {
    void Check(IAnalysisReportInProgress report, IProjectDependencyPath dependencyPath);
  }
}