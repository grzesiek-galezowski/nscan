using System.Xml.Serialization;

namespace NScan.Adapter.ReadingCSharpSolution.ReadingProjects
{
#nullable disable
  [XmlRoot(ElementName = "Reference")]
  public record XmlAssemblyReference
  {
    [XmlElement(ElementName = "HintPath")]
    public string HintPath { get; init; }

    [XmlAttribute(AttributeName = "Include")]
    public string Include { get; init; }
  }
}
