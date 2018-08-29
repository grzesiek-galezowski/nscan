using System.Collections.Generic;
using System.Xml.Serialization;

namespace MyTool.Xml
{
  [XmlRoot(ElementName = "Project")]
  public class XmlProject
  {
    [XmlElement(ElementName = "PropertyGroup")]
    public List<XmlPropertyGroup> PropertyGroup { get; set; }
    [XmlElement(ElementName = "ItemGroup")]
    public List<XmlItemGroup> ItemGroup { get; set; }
    [XmlAttribute(AttributeName = "Sdk")]
    public string Sdk { get; set; }
  }
}