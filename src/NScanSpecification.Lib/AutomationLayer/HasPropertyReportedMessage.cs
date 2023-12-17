using static System.Environment;

namespace NScanSpecification.Lib.AutomationLayer;

public class HasPropertyReportedMessage(string text) : GenericReportedMessage<HasPropertyReportedMessage>(text)
{
  public static HasPropertyReportedMessage HasProperty(
    string projectName, string name, string value)
  {
    return new HasPropertyReportedMessage(TestRuleFormats.FormatHasPropertyRule(projectName, name, value));
  }

  protected override HasPropertyReportedMessage NewInstance(string str)
  {
    return new HasPropertyReportedMessage(str);
  }

  public ReportedMessage ProjectHasAnotherPropertyValue(string projectName, string propertyName, string actualPropertyValue)
  {
    return NewInstance(this + NewLine + $"Project {projectName} has {propertyName}:{actualPropertyValue}");
  }

  public ReportedMessage ProjectDoesNotHavePropertySetExplicitly(string projectName, string propertyName)
  {
    return NewInstance(this + NewLine + $"Project {projectName} does not have {propertyName} set explicitly");
  }
}
