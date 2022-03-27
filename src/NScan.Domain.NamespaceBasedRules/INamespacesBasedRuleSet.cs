using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules;

public interface INamespacesBasedRuleSet
{
  void Add(INamespacesBasedRule rule);
  void Check(IReadOnlyList<INamespaceBasedRuleTarget> dotNetProjects, IAnalysisReportInProgress report);
}