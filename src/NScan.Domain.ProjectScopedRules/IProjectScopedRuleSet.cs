using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public interface IProjectScopedRuleSet
{
  void Add(IProjectScopedRule rule);
  void Check(IReadOnlyList<IProjectScopedRuleTarget> dotNetProjects, IAnalysisReportInProgress report);
}