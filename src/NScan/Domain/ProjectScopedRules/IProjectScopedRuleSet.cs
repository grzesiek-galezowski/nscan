using System.Collections.Generic;
using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.ProjectScopedRules
{
  public interface IProjectScopedRuleSet
  {
    void Add(IProjectScopedRule rule);
    void Check(IReadOnlyList<IProjectScopedRuleTarget> dotNetProjects, IAnalysisReportInProgress report);
  }
}