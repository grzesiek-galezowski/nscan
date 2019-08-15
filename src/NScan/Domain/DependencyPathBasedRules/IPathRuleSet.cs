using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.DependencyPathBasedRules
{
  public interface IPathRuleSet
  {
    void Add(IDependencyRule rule);
    void Check(IPathCache cache, IAnalysisReportInProgress report);
  }
}