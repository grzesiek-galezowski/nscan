using System.Xml.Serialization;

namespace TddXt.NScan.ReadingSolution.Ports
{

  #nullable disable
  [XmlRoot(ElementName = "PropertyGroup")]
  public class XmlPropertyGroup
  {
    [XmlElement(ElementName = "TargetFramework")]
    public string TargetFramework { get; set; }

    [XmlElement(ElementName = "AssemblyName")]
    public string AssemblyName { get; set; }

    [XmlElement(ElementName = "RootNamespace")]
    public string RootNamespace { get; set; }

    [XmlElement(ElementName = "SignAssembly")]
    public string SignAssembly { get; set; }

    [XmlElement(ElementName = "AssemblyOriginatorKeyFile")]
    public string AssemblyOriginatorKeyFile { get; set; }

    [XmlElement(ElementName = "DelaySign")]
    public string DelaySign { get; set; }

    [XmlElement(ElementName = "OutputPath")]
    public string OutputPath { get; set; }

    [XmlAttribute(AttributeName = "Condition")]
    public string Condition { get; set; }
  }

}