using System.Collections.Generic;
using System.Xml.Serialization;

namespace NScan.Adapter.ReadingCSharpSolution.ReadingProjects
{
#nullable disable
  [XmlRoot(ElementName = "ItemGroup")]
  public class XmlItemGroup
  {
    [XmlElement(ElementName = "PackageReference")]
    public List<XmlPackageReference> PackageReferences { get; set; }

    [XmlElement(ElementName = "ProjectReference")]
    public List<XmlProjectReference> ProjectReferences { get; set; }

    [XmlElement(ElementName = "Reference")]
    public List<XmlAssemblyReference> AssemblyReferences { get; set; }
  }
}