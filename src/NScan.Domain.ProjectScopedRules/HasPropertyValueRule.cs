using System.Collections.Generic;
using LanguageExt;
using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public interface IPropertyCheck
{
  void ApplyTo(
    AssemblyName name,
    Map<string, string> properties,
    IAnalysisReportInProgress report);
}

public class HasPropertyValueRule(
  string propertyName,
  Pattern expectedPropertyValuePattern,
  IProjectScopedRuleViolationFactory violationFactory,
  RuleDescription description)
  : IProjectScopedRule, IPropertyCheck
{
  public RuleDescription Description()
  {
    return description;
  }

  public void Check(IProjectScopedRuleTarget project, IAnalysisReportInProgress report)
  {
    project.ValidateProperty(this, report);
  }

  public void ApplyTo(
    AssemblyName name,
    Map<string, string> properties,
    IAnalysisReportInProgress report)
  {
    if (properties.ContainsKey(propertyName))
    {
      var propertyValue = properties[propertyName];
      if (!expectedPropertyValuePattern.IsMatchedBy(propertyValue))
      {
        report.Add(violationFactory.ProjectScopedRuleViolation(description, $"Project {name} has {propertyName}:{propertyValue}"));
      }
    }
    else
    {
      report.Add(violationFactory.ProjectScopedRuleViolation(description, $"Project {name} does not have {propertyName} set explicitly"));
    }
  }
}
