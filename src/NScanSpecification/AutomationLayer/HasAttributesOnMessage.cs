﻿using TddXt.NScan.Specification.EndToEnd.AutomationLayer;
using static System.Environment;

namespace TddXt.NScan.Specification.AutomationLayer
{
  public class HasAttributesOnMessage : GenericReportedMessage<HasAttributesOnMessage>
  {
    private HasAttributesOnMessage(string returnValue) : base(returnValue)
    {
    }

    public HasAttributesOnMessage NonCompliantMethodFound(string className, string methodName)
    {
      return NewInstance(
        $"{this}{NewLine}Method {methodName} in class {className} does not have any attribute");
    }

    protected override HasAttributesOnMessage NewInstance(string str)
    {
      return new HasAttributesOnMessage(str);
    }

    public static HasAttributesOnMessage HasMethodsNotDecoratedWithAttribute(
      string projectName,
      string classNamePattern,
      string methodNamesPattern)
    {
      return new HasAttributesOnMessage(
        TestRuleFormats.FormatHasAttributesOnRule(projectName, classNamePattern,methodNamesPattern));
    }
  }
}