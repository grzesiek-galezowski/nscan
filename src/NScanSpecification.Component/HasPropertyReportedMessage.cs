using NScanSpecification.Lib.AutomationLayer;

namespace NScanSpecification.Component
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
  }
}
