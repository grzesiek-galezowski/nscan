using System.Xml.Serialization;

namespace NScan.Adapter.ReadingCSharpSolution.ReadingProjects
{
#nullable disable
  [XmlRoot(ElementName = "PackageReference")]
  public record XmlPackageReference
  {
    [XmlAttribute(AttributeName = "Include")]
    public string Include { get; init; }

    [XmlAttribute(AttributeName = "Version")]
    public string Version { get; init; }
  }
}
