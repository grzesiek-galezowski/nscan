using System.Collections.Generic;

namespace TddXt.NScan.Domain
{
  public interface IProjectScopedRule
  {
    void Check(IReadOnlyList<ISourceCodeFile> files, string rootNamespace, IAnalysisReportInProgress report);
  }
}