using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRules
{
  public interface IPathCache
  {
    void BuildStartingFrom(params IDependencyPathBasedRuleTarget[] rootProjects);
    void Check(IDependencyRule rule, IAnalysisReportInProgress report);
  }
}