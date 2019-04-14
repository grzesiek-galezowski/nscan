namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public class XmlClassBuilder
  {
    public static XmlClassBuilder Class(string name)
    {
      return new XmlClassBuilder();
    }

    public XmlClassBuilder With(params XmlMethodBuilder[] methodBuilders)
    {
      return this;
    }
  }
}