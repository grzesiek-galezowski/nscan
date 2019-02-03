using System.Collections.Generic;
using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.ProjectScopedRules
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
        sourceCodeFile.EvaluateNamespacesCorrectness(report, ToString());
      }
    }

    public override string ToString()
    {
      return _ruleDto.ProjectAssemblyNamePattern.Description() + " " + _ruleDto.RuleName;
    }
  }
}