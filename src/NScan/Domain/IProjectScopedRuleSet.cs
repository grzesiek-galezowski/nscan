using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public interface IProjectScopedRuleSet
  {
    void Add(IProjectScopedRule rule);
    void Check(IReadOnlyList<IProjectScopedRuleTarget> dotNetProjects, IAnalysisReportInProgress report);
  }
}