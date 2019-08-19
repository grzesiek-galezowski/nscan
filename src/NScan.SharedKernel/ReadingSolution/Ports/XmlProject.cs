using System.Collections.Generic;
using System.Xml.Serialization;
using AtmaFileSystem;

namespace NScan.SharedKernel.ReadingSolution.Ports
{
#nullable disable
  [XmlRoot(ElementName = "Project")]
  public class XmlProject
  {
    [XmlElement(ElementName = "PropertyGroup")]
    public List<XmlPropertyGroup> PropertyGroups { get; set; }

    [XmlElement(ElementName = "ItemGroup")]
    public List<XmlItemGroup> ItemGroups { get; set; }

    [XmlAttribute(AttributeName = "Sdk")]
    public string Sdk { get; set; }

    [XmlIgnore]
    public AbsoluteFilePath AbsolutePath { get; set; }

    [XmlIgnore]
    public List<XmlSourceCodeFile> SourceCodeFiles { get; } = new List<XmlSourceCodeFile>();
  }
}