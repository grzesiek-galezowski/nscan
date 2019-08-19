using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.Domain.NamespaceBasedRules
{
  public interface INamespacesBasedRuleSet
  {
    void Add(INamespacesBasedRule rule);
    void Check(IReadOnlyList<INamespaceBasedRuleTarget> dotNetProjects, IAnalysisReportInProgress report);
  }
}