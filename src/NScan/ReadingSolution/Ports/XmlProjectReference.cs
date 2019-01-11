using System.Xml.Serialization;

namespace TddXt.NScan.ReadingSolution.Ports
{
  [XmlRoot(ElementName = "ProjectReference")]
  public class XmlProjectReference
  {
    [XmlAttribute(AttributeName = "Include")]
    public string Include { get; set; }
  }
}