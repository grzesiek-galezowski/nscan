using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
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