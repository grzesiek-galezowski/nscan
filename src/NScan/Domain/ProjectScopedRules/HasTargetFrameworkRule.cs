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
    private readonly IProjectScopedRuleViolationFactory _violationFactory;
    private readonly string _ruleDescription;

    public HasTargetFrameworkRule(Pattern projectAssemblyNamePattern, string targetFramework,
      IProjectScopedRuleViolationFactory violationFactory, string ruleDescription)
    {
      _projectAssemblyNamePattern = projectAssemblyNamePattern;
      _targetFramework = targetFramework;
      _violationFactory = violationFactory;
      _ruleDescription = ruleDescription;
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
      //var projectScopedRuleViolation = _violationFactory.ProjectScopedRuleViolation(_ruleDescription, "LALALALA");
      //report.Add(projectScopedRuleViolation);
    }

    public override string ToString()
    {
      return $"{_ruleDescription} {_targetFramework}";
    }
  }
}