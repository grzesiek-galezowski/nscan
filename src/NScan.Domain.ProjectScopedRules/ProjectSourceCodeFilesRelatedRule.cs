using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public class ProjectSourceCodeFilesRelatedRule : IProjectScopedRule, IProjectFilesetScopedRule
{
  private readonly ISourceCodeFileContentCheck _fileContentCheck;
  private readonly RuleDescription _ruleDescription;

  public ProjectSourceCodeFilesRelatedRule(
    RuleDescription description,
    ISourceCodeFileContentCheck fileContentCheck)
  {
    _fileContentCheck = fileContentCheck;
    _ruleDescription = description;
  }

  public RuleDescription Description()
  {
    return _ruleDescription;
  }

  public void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report)
  {
    project.AnalyzeFiles(this, report);
  }

  public void Check(IReadOnlyList<ISourceCodeFileInNamespace> sourceCodeFiles, IAnalysisReportInProgress report)
  {
    foreach (var sourceCodeFile in sourceCodeFiles)
    {
      _fileContentCheck.ApplyTo(sourceCodeFile, _ruleDescription, report);
    }
    report.FinishedEvaluatingRule(_ruleDescription);
  }
}