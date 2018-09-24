using System.Collections.Generic;
using System.Xml.Serialization;

namespace NScanRoot.Xml
{
  [XmlRoot(ElementName = "ItemGroup")]
  public class XmlItemGroup
  {
    [XmlElement(ElementName = "PackageReference")]
    public List<XmlPackageReference> PackageReference { get; set; }
    [XmlElement(ElementName = "ProjectReference")]
    public List<XmlProjectReference> ProjectReference { get; set; }
  }
}