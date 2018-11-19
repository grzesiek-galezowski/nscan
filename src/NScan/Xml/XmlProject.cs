using System.Collections.Generic;
using System.Xml.Serialization;

namespace TddXt.NScan.Xml
{
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
    public string AbsolutePath { get; set; }

    [XmlIgnore]
    public List<SourceCodeFile> SourceCodeFiles { get; }= new List<SourceCodeFile>();
  }
}