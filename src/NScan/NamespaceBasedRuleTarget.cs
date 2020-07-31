using System.Collections.Generic;
using NScan.NamespaceBasedRules;
using NScan.SharedKernel;

namespace NScan.Domain
{
  public class NamespaceBasedRuleTarget : INamespaceBasedRuleTarget //bug move this to a namespace-specific project
  {
    private readonly string _assemblyName;
    private readonly IReadOnlyList<ISourceCodeFile> _sourceCodeFiles;
    private readonly INamespacesDependenciesCache _namespacesDependenciesCache;

    public NamespaceBasedRuleTarget(
      string assemblyName, 
      IReadOnlyList<ISourceCodeFile> sourceCodeFiles,
      INamespacesDependenciesCache namespacesDependenciesCache)
    {
      _assemblyName = assemblyName;
      _sourceCodeFiles = sourceCodeFiles;
      _namespacesDependenciesCache = namespacesDependenciesCache;
    }

    public void RefreshNamespacesCache()
    {
      foreach (var sourceCodeFile in _sourceCodeFiles)
      {
        sourceCodeFile.AddNamespaceMappingTo(_namespacesDependenciesCache);
      }
    }

    public void Evaluate(INamespacesBasedRule rule, IAnalysisReportInProgress report)
    {
      rule.Evaluate(_assemblyName, _namespacesDependenciesCache, report);
    }
  }
}