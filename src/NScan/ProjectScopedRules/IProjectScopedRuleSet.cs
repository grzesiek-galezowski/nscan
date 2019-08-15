using System.Collections.Generic;
using NScan.SharedKernel.SharedKernel;

namespace NScan.Domain.Domain.ProjectScopedRules
{
  public interface IProjectScopedRuleSet
  {
    void Add(IProjectScopedRule rule);
    void Check(IReadOnlyList<IProjectScopedRuleTarget> dotNetProjects, IAnalysisReportInProgress report);
  }
}