using System.Collections.Generic;
using GlobExpressions;
using NScan.Lib;
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

  public class HasPropertyValueRule : IProjectScopedRule, IPropertyCheck
  {
    private readonly IProjectScopedRuleViolationFactory _violationFactory;
    private readonly string _ruleDescription;
    private readonly string _propertyName;
    private readonly Pattern _expectedPropertyValuePattern;

    public HasPropertyValueRule(
      string propertyName,
      Pattern expectedPropertyValuePattern,
      IProjectScopedRuleViolationFactory violationFactory,
      string ruleDescription)
    {
      _violationFactory = violationFactory;
      _ruleDescription = ruleDescription;
      _propertyName = propertyName;
      _expectedPropertyValuePattern = expectedPropertyValuePattern;
    }

    public void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report)
    {
      project.ValidateProperty(this, report);
    }

    public void ApplyTo(string assemblyName, IReadOnlyDictionary<string, string> properties, IAnalysisReportInProgress report)
    {
      if (properties.ContainsKey(_propertyName))
      {
        var propertyValue = properties[_propertyName];
        if (!_expectedPropertyValuePattern.IsMatch(propertyValue))
        {
          report.Add(_violationFactory.ProjectScopedRuleViolation(
            _ruleDescription, $"Project {assemblyName} has {_propertyName}:{propertyValue}"));
        }
      }
      else
      {
        report.Add(_violationFactory.ProjectScopedRuleViolation(
          _ruleDescription, $"Project {assemblyName} does not have {_propertyName} set explicitly"));
      }
    }

    public override string ToString()
    {
      return _ruleDescription;
    }
  }
}
