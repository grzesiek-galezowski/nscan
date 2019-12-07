using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.Domain.Root
{
  public interface ISpecificKindOfRuleAnalysis<in T>
  {
    void PerformOn(ISolution solution, IAnalysisReportInProgress analysisReportInProgress);
    void Add(IEnumerable<T> rules);
  }
}