using System;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public class MethodDeclarationBuilder
  {
    public static MethodDeclarationBuilder Method(string name)
    {
      throw new NotImplementedException("TODO: add name to generate the method");
      return new MethodDeclarationBuilder();
    }

    public MethodDeclarationBuilder DecoratedWithAttribute(string attributeName)
    {
      return this;
    }
  }
}