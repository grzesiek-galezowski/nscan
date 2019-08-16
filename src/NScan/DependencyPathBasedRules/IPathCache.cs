using NScan.SharedKernel;

namespace NScan.Domain.DependencyPathBasedRules
{
  public interface IPathCache
  {
    void BuildStartingFrom(params IDependencyPathBasedRuleTarget[] rootProjects);
    void Check(IDependencyRule rule, IAnalysisReportInProgress report);
  }
}