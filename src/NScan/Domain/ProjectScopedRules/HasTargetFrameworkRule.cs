using System.Collections.Generic;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.ProjectScopedRules
{
  public class HasTargetFrameworkRule : IProjectScopedRule
  {
    private readonly Pattern _projectAssemblyNamePattern;
    private readonly string _targetFramework;

    public HasTargetFrameworkRule(Pattern projectAssemblyNamePattern, string targetFramework)
    {
      _projectAssemblyNamePattern = projectAssemblyNamePattern;
      _targetFramework = targetFramework;
    }

    public void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report)
    {
      throw new System.NotImplementedException();
    }

    public void Check(IReadOnlyList<ISourceCodeFileInNamespace> sourceCodeFiles, IAnalysisReportInProgress report)
    {
      throw new System.NotImplementedException();
    }
  }
}