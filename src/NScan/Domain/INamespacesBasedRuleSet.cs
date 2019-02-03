using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public interface INamespacesBasedRuleSet
  {
    void Add(INamespacesBasedRule rule);
    void Check(IReadOnlyList<INamespaceBasedRuleTarget> dotNetProjects, IAnalysisReportInProgress report);
  }
}