using System.Collections.Generic;
using LanguageExt;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public interface IProjectScopedRuleSet
{
  void Add(IProjectScopedRule rule);
  void Check(Seq<IProjectScopedRuleTarget> dotNetProjects, IAnalysisReportInProgress report);
}
