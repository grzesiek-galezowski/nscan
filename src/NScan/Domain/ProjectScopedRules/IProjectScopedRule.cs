using System.Collections.Generic;
using TddXt.NScan.Domain.SharedKernel;

namespace TddXt.NScan.Domain.ProjectScopedRules
{
  public interface IProjectFilesetScopedRule
  {
    void Check(IReadOnlyList<ISourceCodeFileInNamespace> sourceCodeFiles, IAnalysisReportInProgress report);
  }

  public interface IProjectScopedRule
  {
    void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report);
  }
}