using System;
using System.Collections.Generic;
using TddXt.NScan.RuleInputData;

namespace TddXt.NScan.Domain
{
  public class NoCircularUsingsRule : IProjectScopedRule, INamespacesBasedRule
  {
      private readonly NoCircularUsingsRuleComplementDto _ruleDto;

      public NoCircularUsingsRule(NoCircularUsingsRuleComplementDto ruleDto)
      {
          _ruleDto = ruleDto;
      }

    public void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report)
    {
      throw new NotImplementedException();
    }

    public void Check(IReadOnlyList<ISourceCodeFile> sourceCodeFiles, IAnalysisReportInProgress report)
    {
      throw new NotImplementedException();
    }
  }
}