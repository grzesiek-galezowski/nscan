namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public class XmlMethodBuilder
  {
    public static XmlMethodBuilder Method(string name)
    {
      return new XmlMethodBuilder();
    }

    public XmlMethodBuilder DecoratedWithAttribute(string attributeName)
    {
      return this;
    }
  }
}