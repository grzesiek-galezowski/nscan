using System.Collections.Generic;
using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public class ProjectScopedRuleTarget : IProjectScopedRuleTarget
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
      return pattern.IsMatchedBy(_assemblyName);
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

    public void AddInfoAboutMatchingPatternTo(IAnalysisReportInProgress report)
    {
      report.StartedCheckingTarget(_assemblyName);
    }
  }
}
