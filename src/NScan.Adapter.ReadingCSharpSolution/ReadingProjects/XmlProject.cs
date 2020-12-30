using System.Collections.Generic;
using System.Xml.Serialization;
using AtmaFileSystem;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.Adapter.ReadingCSharpSolution.ReadingProjects
{
#nullable disable
  [XmlRoot(ElementName = "Project")]
  public record XmlProject
  {
    [XmlElement(ElementName = "PropertyGroup")]
    public List<XmlPropertyGroup> PropertyGroups { get; init; }

    [XmlElement(ElementName = "ItemGroup")]
    public List<XmlItemGroup> ItemGroups { get; init; }

    [XmlAttribute(AttributeName = "Sdk")]
    public string Sdk { get; init; }

    [XmlIgnore]
    public AbsoluteFilePath AbsolutePath { get; init; }

    [XmlIgnore]
    public List<SourceCodeFileDto> SourceCodeFiles { get; } = new();
  }
}
