using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain.ProjectScopedRules
{
  public interface ITargetFrameworkCheck
  {
    void ApplyTo(string targetFramework, IAnalysisReportInProgress report);
  }


  public class HasTargetFrameworkRule : IProjectScopedRule, ITargetFrameworkCheck
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
      if (project.HasProjectAssemblyNameMatching(_projectAssemblyNamePattern))
      {
        project.ValidateTargetFrameworkWith((ITargetFrameworkCheck) this, report);
      }
    }

    public void ApplyTo(string targetFramework, IAnalysisReportInProgress report)
    {
      throw new System.NotImplementedException();
    }
  }
}