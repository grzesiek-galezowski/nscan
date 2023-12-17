using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules;

public class NamespaceBasedRuleTarget(
  AssemblyName assemblyName,
  IReadOnlyList<ISourceCodeFileUsingNamespaces> sourceCodeFiles,
  INamespacesDependenciesCache namespacesDependenciesCache)
  : INamespaceBasedRuleTarget
{
  public void RefreshNamespacesCache()
  {
    foreach (var sourceCodeFile in sourceCodeFiles)
    {
      sourceCodeFile.AddNamespaceMappingTo(namespacesDependenciesCache);
    }
  }

  public void Evaluate(INamespacesBasedRule rule, IAnalysisReportInProgress report)
  {
    rule.Evaluate(assemblyName, namespacesDependenciesCache, report);
  }
}
