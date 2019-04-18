using System.Collections.Generic;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public class XmlClassBuilder
  {
    private readonly List<XmlMethodBuilder> _methods = new List<XmlMethodBuilder>();

    public static XmlClassBuilder Class(string name)
    {
      return new XmlClassBuilder();
    }

    public XmlClassBuilder With(params XmlMethodBuilder[] methodBuilders)
    {
      _methods.AddRange(methodBuilders);
      return this;
    }
  }
}