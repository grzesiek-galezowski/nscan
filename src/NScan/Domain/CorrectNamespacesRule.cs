using System.Collections.Generic;
using TddXt.NScan.RuleInputData;

namespace TddXt.NScan.Domain
{
  public class CorrectNamespacesRule : IProjectScopedRule
  {
    private readonly CorrectNamespacesRuleComplementDto _ruleDto;

    public CorrectNamespacesRule(CorrectNamespacesRuleComplementDto ruleDto)
    {
      _ruleDto = ruleDto;
    }

    public void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report)
    {
      if (project.HasProjectAssemblyNameMatching(_ruleDto.ProjectAssemblyNamePattern))
      {
        project.AnalyzeFiles(this, report);
      }
    }

    public void Check(IReadOnlyList<ISourceCodeFile> sourceCodeFiles, IAnalysisReportInProgress report)
    {
      foreach (var sourceCodeFile in sourceCodeFiles)
      {
        sourceCodeFile.EvaluateNamespacesCorrectness(report);
      }
    }
  }
}