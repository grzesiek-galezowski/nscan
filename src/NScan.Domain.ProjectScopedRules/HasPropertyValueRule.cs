using System.Collections.Generic;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public interface IPropertyCheck
  {
    void ApplyTo(
      string assemblyName, 
      IReadOnlyDictionary<string, string> properties, 
      IAnalysisReportInProgress report);
  }

  //bug more generic name?
  public class HasPropertyValueRule : IProjectScopedRule, IPropertyCheck
  {
    private readonly string _expectedPropertyValue;
    private readonly IProjectScopedRuleViolationFactory _violationFactory;
    private readonly string _ruleDescription;
    private readonly string _propertyName;

    public HasPropertyValueRule(
      string propertyName, 
      string expectedPropertyValue,
      IProjectScopedRuleViolationFactory violationFactory,
      string ruleDescription)
    {
      _expectedPropertyValue = expectedPropertyValue;
      _violationFactory = violationFactory;
      _ruleDescription = ruleDescription;
      _propertyName = propertyName;
    }

    public void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report)
    {
      project.ValidateProperty(this, report);
    }

    public void ApplyTo(string assemblyName, IReadOnlyDictionary<string, string> properties, IAnalysisReportInProgress report)
    {
      var propertyValue = properties[_propertyName];
      if (propertyValue != _expectedPropertyValue)
      {
        var projectScopedRuleViolation = _violationFactory.ProjectScopedRuleViolation(
          _ruleDescription, $"Project {assemblyName} has {_propertyName}:{propertyValue}");
        report.Add(projectScopedRuleViolation);
      }
    }

    public override string ToString()
    {
      return _ruleDescription;
    }
  }
}
