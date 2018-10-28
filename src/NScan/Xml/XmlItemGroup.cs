using System.Collections.Generic;
using System.Xml.Serialization;

namespace TddXt.NScan.Xml
{
  [XmlRoot(ElementName = "ItemGroup")]
  public class XmlItemGroup
  {
    [XmlElement(ElementName = "PackageReference")]
    public IReadOnlyList<XmlPackageReference> PackageReferences { get; set; }
    [XmlElement(ElementName = "ProjectReference")]
    public IReadOnlyList<XmlProjectReference> ProjectReferences { get; set; }
  }
}