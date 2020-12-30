using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public interface ITargetFrameworkCheck
  {
    void ApplyTo(string assemblyName, string targetFramework, IAnalysisReportInProgress report);
  }

  public class HasTargetFrameworkRule : IProjectScopedRule, ITargetFrameworkCheck
  {
    private readonly string _expectedTargetFramework;
    private readonly IProjectScopedRuleViolationFactory _violationFactory;
    private readonly string _ruleDescription;

    public HasTargetFrameworkRule(
      string expectedTargetFramework,
      IProjectScopedRuleViolationFactory violationFactory, 
      string ruleDescription)
    {
      _expectedTargetFramework = expectedTargetFramework;
      _violationFactory = violationFactory;
      _ruleDescription = ruleDescription;
    }

    public void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report)
    {
      project.ValidateTargetFrameworkWith(this, report);
    }

    public void ApplyTo(string assemblyName, string targetFramework, IAnalysisReportInProgress report)
    {
      if (targetFramework != _expectedTargetFramework)
      {
        var projectScopedRuleViolation = _violationFactory.ProjectScopedRuleViolation(
          ToString(), $"Project {assemblyName} has target framework {targetFramework}");
        report.Add(projectScopedRuleViolation);
      }
    }

    public override string ToString()
    {
      return _ruleDescription;
    }
  }
}
