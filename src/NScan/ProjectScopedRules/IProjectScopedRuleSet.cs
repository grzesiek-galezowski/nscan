using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.Domain.ProjectScopedRules
{
  public interface IProjectScopedRuleSet
  {
    void Add(IProjectScopedRule rule);
    void Check(IReadOnlyList<IProjectScopedRuleTarget> dotNetProjects, IAnalysisReportInProgress report);
  }
}