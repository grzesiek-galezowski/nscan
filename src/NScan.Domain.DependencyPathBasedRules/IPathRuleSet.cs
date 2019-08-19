using NScan.SharedKernel;

namespace NScan.Domain.DependencyPathBasedRules
{
  public interface IPathRuleSet
  {
    void Add(IDependencyRule rule);
    void Check(IPathCache cache, IAnalysisReportInProgress report);
  }
}