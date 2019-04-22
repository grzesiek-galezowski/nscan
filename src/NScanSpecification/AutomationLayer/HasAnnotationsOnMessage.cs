using System;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Specification.AutomationLayer
{
  public class HasAnnotationsOnMessage : GenericReportedMessage<HasAnnotationsOnMessage>
  {
    public HasAnnotationsOnMessage(string returnValue) : base(returnValue)
    {
    }

    public HasAnnotationsOnMessage NonCompliantMethodFound(string className, string methodName)
    {
      throw new NotImplementedException();
    }

    protected override HasAnnotationsOnMessage NewInstance(string str)
    {
      return new HasAnnotationsOnMessage(str);
    }

    public static HasAnnotationsOnMessage HasMethodsNotDecoratedWithAttribute(
      string projectName,
      string classNamePattern,
      string methodNamesPattern)
    {
      return new HasAnnotationsOnMessage(projectName + " " + RuleNames.HasAnnotationsOn + " " + classNamePattern + ":" + methodNamesPattern);
    }
  }
}