using System.Collections.Generic;
using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.ProjectScopedRules
{
  public interface IProjectScopedRule
  {
    void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report);
    void Check(IReadOnlyList<ISourceCodeFile> sourceCodeFiles, IAnalysisReportInProgress report);
  }
}