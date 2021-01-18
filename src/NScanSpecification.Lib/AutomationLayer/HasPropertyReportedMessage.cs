using static System.Environment;

namespace NScanSpecification.Lib.AutomationLayer
{
  public class HasPropertyReportedMessage : GenericReportedMessage<HasPropertyReportedMessage>
  {
    public static HasPropertyReportedMessage HasProperty(
      string projectName, string name, string value)
    {
      return new(TestRuleFormats.FormatHasPropertyRule(projectName, name, value));
    }

    public HasPropertyReportedMessage(string text) : base(text)
    {
    }

    protected override HasPropertyReportedMessage NewInstance(string str)
    {
      return new(str);
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
}
