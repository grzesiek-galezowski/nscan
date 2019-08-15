using System.Xml.Serialization;

namespace NScan.SharedKernel.ReadingSolution.Ports
{
#nullable disable
  [XmlRoot(ElementName = "PackageReference")]
  public class XmlPackageReference
  {
    [XmlAttribute(AttributeName = "Include")]
    public string Include { get; set; }

    [XmlAttribute(AttributeName = "Version")]
    public string Version { get; set; }
  }
}