using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public interface IProjectScopedRule
  {
    void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report);
    void Check(IReadOnlyList<ISourceCodeFile> sourceCodeFiles, IAnalysisReportInProgress report);
  }
}