using static System.Environment;

namespace NScanSpecification.Lib.AutomationLayer
{
  public class HasAttributesOnMessage : GenericReportedMessage<HasAttributesOnMessage>
  {
    private HasAttributesOnMessage(string text) : base(text)
    {
    }

    public HasAttributesOnMessage NonCompliantMethodFound(string className, string methodName)
    {
      return NewInstance(
        $"{this}{NewLine}Method {methodName} in class {className} does not have any attribute");
    }

    protected override HasAttributesOnMessage NewInstance(string str)
    {
      return new(str);
    }

    public static HasAttributesOnMessage HasMethodsNotDecoratedWithAttribute(
      string projectName,
      string classNamePattern,
      string methodNamesPattern)
    {
      return new(
        TestRuleFormats.FormatHasAttributesOnRule(projectName, classNamePattern,methodNamesPattern));
    }
  }
}
