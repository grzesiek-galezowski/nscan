﻿using System.Collections.Generic;
using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public class ProjectScopedRuleTarget : IProjectScopedRuleTarget
{
  private readonly IReadOnlyList<ISourceCodeFileInNamespace> _sourceCodeFiles;
  private readonly IReadOnlyDictionary<string, string> _properties;
  private readonly AssemblyName _assemblyName;

  public ProjectScopedRuleTarget(
    AssemblyName name, 
    IReadOnlyList<ISourceCodeFileInNamespace> sourceCodeFiles,
    IReadOnlyDictionary<string, string> properties)
  {
    _sourceCodeFiles = sourceCodeFiles;
    _properties = properties;
    _assemblyName = name;
  }

  public void AnalyzeFiles(IProjectFilesetScopedRule rule, IAnalysisReportInProgress report)
  {
    rule.Check(_sourceCodeFiles, report);
  }

  public bool HasProjectAssemblyNameMatching(Pattern pattern)
  {
    return _assemblyName.Matches(pattern);
  }

  public void ValidateProperty(
    IPropertyCheck propertyCheck,
    IAnalysisReportInProgress analysisReportInProgress)
  {
    propertyCheck.ApplyTo(_assemblyName, _properties, analysisReportInProgress);
  }

  public void AddInfoAboutMatchingPatternTo(IAnalysisReportInProgress report)
  {
    report.StartedCheckingTarget(_assemblyName);
  }
}