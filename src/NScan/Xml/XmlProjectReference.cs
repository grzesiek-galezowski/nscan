using System.Xml.Serialization;

namespace TddXt.NScan.Xml
{
  [XmlRoot(ElementName = "ProjectReference")]
  public class XmlProjectReference
  {
    [XmlAttribute(AttributeName = "Include")]
    public string Include { get; set; }
  }
}