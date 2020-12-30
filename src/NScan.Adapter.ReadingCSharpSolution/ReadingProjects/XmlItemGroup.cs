using System.Collections.Generic;
using System.Xml.Serialization;

namespace NScan.Adapter.ReadingCSharpSolution.ReadingProjects
{
#nullable disable
  [XmlRoot(ElementName = "ItemGroup")]
  public record XmlItemGroup
  {
    [XmlElement(ElementName = "PackageReference")]
    public List<XmlPackageReference> PackageReferences { get; init; }

    [XmlElement(ElementName = "ProjectReference")]
    public List<XmlProjectReference> ProjectReferences { get; init; }

    [XmlElement(ElementName = "Reference")]
    public List<XmlAssemblyReference> AssemblyReferences { get; init; }
  }
}
