using System.Collections.Generic;
using LanguageExt;
using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public class ProjectScopedRuleTarget(
  AssemblyName name,
  IReadOnlyList<ISourceCodeFileInNamespace> sourceCodeFiles,
  HashMap<string, string> properties)
  : IProjectScopedRuleTarget
{
  public void AnalyzeFiles(IProjectFilesetScopedRule rule, IAnalysisReportInProgress report)
  {
    rule.Check(sourceCodeFiles, report);
  }

  public bool HasProjectAssemblyNameMatching(Pattern pattern)
  {
    return name.Matches(pattern);
  }

  public void ValidateProperty(
    IPropertyCheck propertyCheck,
    IAnalysisReportInProgress analysisReportInProgress)
  {
    propertyCheck.ApplyTo(name, properties, analysisReportInProgress);
  }

  public void AddInfoAboutMatchingPatternTo(IAnalysisReportInProgress report)
  {
    report.StartedCheckingTarget(name);
  }
}
