using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.DependencyPathBasedRules
{
  public interface IPathCache
  {
    void BuildStartingFrom(params IDependencyPathBasedRuleTarget[] rootProjects);
    void Check(IDependencyRule rule, IAnalysisReportInProgress report);
  }
}