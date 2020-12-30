using System.Xml.Serialization;

namespace NScan.Adapter.ReadingCSharpSolution.ReadingProjects
{

  #nullable disable
  [XmlRoot(ElementName = "PropertyGroup")]
  public record XmlPropertyGroup
  {
    [XmlElement(ElementName = "TargetFramework")]
    public string TargetFramework { get; init; }

    [XmlElement(ElementName = "AssemblyName")]
    public string AssemblyName { get; init; }

    [XmlElement(ElementName = "RootNamespace")]
    public string RootNamespace { get; init; }

    [XmlElement(ElementName = "SignAssembly")]
    public string SignAssembly { get; init; }

    [XmlElement(ElementName = "AssemblyOriginatorKeyFile")]
    public string AssemblyOriginatorKeyFile { get; init; }

    [XmlElement(ElementName = "DelaySign")]
    public string DelaySign { get; init; }

    [XmlElement(ElementName = "OutputPath")]
    public string OutputPath { get; init; }

    [XmlAttribute(AttributeName = "Condition")]
    public string Condition { get; init; }
  }

}
