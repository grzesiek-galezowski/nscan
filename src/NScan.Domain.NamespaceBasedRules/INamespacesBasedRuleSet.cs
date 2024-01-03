using System.Collections.Generic;
using LanguageExt;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules;

public interface INamespacesBasedRuleSet
{
  void Add(INamespacesBasedRule rule);
  void Check(Seq<INamespaceBasedRuleTarget> dotNetProjects, IAnalysisReportInProgress report);
}
