using System.Xml.Serialization;

namespace TddXt.NScan.Xml
{
  [XmlRoot(ElementName = "Reference")]
  public class XmlAssemblyReference
  {
    [XmlElement(ElementName = "HintPath")]
    public string HintPath { get; set; }
    [XmlAttribute(AttributeName = "Include")]
    public string Include { get; set; }
  }
}