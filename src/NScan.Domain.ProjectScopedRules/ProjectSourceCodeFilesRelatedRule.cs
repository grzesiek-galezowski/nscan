using System.Collections.Generic;
using LanguageExt;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public class ProjectSourceCodeFilesRelatedRule(
  RuleDescription description,
  ISourceCodeFileContentCheck fileContentCheck)
  : IProjectScopedRule, IProjectFilesetScopedRule
{
  public RuleDescription Description()
  {
    return description;
  }

  public void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report)
  {
    project.AnalyzeFiles(this, report);
  }

  public void Check(Seq<ISourceCodeFileInNamespace> sourceCodeFiles, IAnalysisReportInProgress report)
  {
    foreach (var sourceCodeFile in sourceCodeFiles)
    {
      fileContentCheck.ApplyTo(sourceCodeFile, description, report);
    }
    report.FinishedEvaluatingRule(description);
  }
}
