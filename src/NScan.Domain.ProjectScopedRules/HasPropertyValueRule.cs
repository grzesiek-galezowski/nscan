using System.Collections.Generic;
using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules
{
  public interface IPropertyCheck
  {
    void ApplyTo(AssemblyName name,
      IReadOnlyDictionary<string, string> properties,
      IAnalysisReportInProgress report);
  }

  public class HasPropertyValueRule : IProjectScopedRule, IPropertyCheck
  {
    private readonly IProjectScopedRuleViolationFactory _violationFactory;
    private readonly string _propertyName;
    private readonly Pattern _expectedPropertyValuePattern;
    private readonly RuleDescription _ruleDescription;

    public HasPropertyValueRule(
      string propertyName,
      Pattern expectedPropertyValuePattern,
      IProjectScopedRuleViolationFactory violationFactory, 
      RuleDescription description)
    {
      _violationFactory = violationFactory;
      _propertyName = propertyName;
      _expectedPropertyValuePattern = expectedPropertyValuePattern;
      _ruleDescription = description;
    }

    public RuleDescription Description()
    {
      return _ruleDescription;
    }

    public void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report)
    {
      project.ValidateProperty(this, report);
    }

    public void ApplyTo(
      AssemblyName name, 
      IReadOnlyDictionary<string, string> properties,
      IAnalysisReportInProgress report)
    {
      if (properties.ContainsKey(_propertyName))
      {
        var propertyValue = properties[_propertyName];
        if (!_expectedPropertyValuePattern.IsMatchedBy(propertyValue))
        {
          report.Add(_violationFactory.ProjectScopedRuleViolation(_ruleDescription, $"Project {name} has {_propertyName}:{propertyValue}"));
        }
      }
      else
      {
        report.Add(_violationFactory.ProjectScopedRuleViolation(_ruleDescription, $"Project {name} does not have {_propertyName} set explicitly"));
      }
    }
  }
}
