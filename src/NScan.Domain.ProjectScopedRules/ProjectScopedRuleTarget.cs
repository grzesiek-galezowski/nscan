using System.Collections.Generic;
using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public class ProjectScopedRuleTarget : IProjectScopedRuleTarget //bug move
  {
    private readonly string _assemblyName;
    private readonly IReadOnlyList<ISourceCodeFileInNamespace> _sourceCodeFiles;
    private readonly IReadOnlyDictionary<string, string> _properties;

    public ProjectScopedRuleTarget(string assemblyName,
      IReadOnlyList<ISourceCodeFileInNamespace> sourceCodeFiles, 
      IReadOnlyDictionary<string, string> properties)
    {
      _assemblyName = assemblyName;
      _sourceCodeFiles = sourceCodeFiles;
      _properties = properties;
    }

    public void AnalyzeFiles(IProjectFilesetScopedRule rule, IAnalysisReportInProgress report)
    {
      rule.Check(_sourceCodeFiles, report);
    }

    public bool HasProjectAssemblyNameMatching(Pattern pattern)
    {
      return pattern.IsMatch(_assemblyName);
    }

    public void ValidateProperty(
      IPropertyCheck propertyCheck,
      IAnalysisReportInProgress analysisReportInProgress)
    {
      propertyCheck.ApplyTo(
        _assemblyName, 
        _properties, 
        analysisReportInProgress);
    }
  }
}
