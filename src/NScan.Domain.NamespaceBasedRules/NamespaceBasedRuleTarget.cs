using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.NamespaceBasedRules
{
  public class NamespaceBasedRuleTarget : INamespaceBasedRuleTarget
  {
    private readonly IReadOnlyList<ISourceCodeFileUsingNamespaces> _sourceCodeFiles;
    private readonly INamespacesDependenciesCache _namespacesDependenciesCache;
    private readonly AssemblyName _projectAssemblyName;

    public NamespaceBasedRuleTarget(
      AssemblyName assemblyName,
      IReadOnlyList<ISourceCodeFileUsingNamespaces> sourceCodeFiles,
      INamespacesDependenciesCache namespacesDependenciesCache)
    {
      _sourceCodeFiles = sourceCodeFiles;
      _namespacesDependenciesCache = namespacesDependenciesCache;
      _projectAssemblyName = assemblyName;
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
      rule.Evaluate(_projectAssemblyName, _namespacesDependenciesCache, report);
    }
  }
}
