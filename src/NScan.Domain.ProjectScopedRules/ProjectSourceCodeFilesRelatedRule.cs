using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public class ProjectSourceCodeFilesRelatedRule : IProjectScopedRule, IProjectFilesetScopedRule
  {
    private readonly string _ruleDescription;
    private readonly ISourceCodeFileContentCheck _fileContentCheck;

    public ProjectSourceCodeFilesRelatedRule(
      string ruleDescription,
      ISourceCodeFileContentCheck fileContentCheck)
    {
      _ruleDescription = ruleDescription;
      _fileContentCheck = fileContentCheck;
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
      report.FinishedEvaluatingRule(/* bug investigate */new RuleDescription(_ruleDescription));
    }

    public override string ToString()
    {
      return _ruleDescription;
    }
  }
}
