using NScan.SharedKernel;

namespace NScan.DependencyPathBasedRules
{
  public interface IPathRuleSet
  {
    void Add(IDependencyRule rule);
    void Check(IPathCache cache, IAnalysisReportInProgress report);
  }
}