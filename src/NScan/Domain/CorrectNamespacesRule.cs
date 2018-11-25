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

    public void Check(IReadOnlyList<ISourceCodeFile> files, string rootNamespace, IAnalysisReportInProgress report)
    {
      throw new System.NotImplementedException();
    }
  }
}