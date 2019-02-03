using System.Collections.Generic;
using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.NamespaceBasedRules
{
  public interface INamespacesBasedRuleSet
  {
    void Add(INamespacesBasedRule rule);
    void Check(IReadOnlyList<INamespaceBasedRuleTarget> dotNetProjects, IAnalysisReportInProgress report);
  }
}