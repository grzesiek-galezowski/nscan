using System.Xml.Serialization;

namespace TddXt.NScan.Xml
{
  [XmlRoot(ElementName = "PackageReference")]
  public class XmlPackageReference
  {
    [XmlAttribute(AttributeName = "Include")]
    public string Include { get; set; }
    [XmlAttribute(AttributeName = "Version")]
    public string Version { get; set; }
  }
}