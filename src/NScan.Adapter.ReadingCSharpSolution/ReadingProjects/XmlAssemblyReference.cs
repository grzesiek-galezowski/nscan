using System.Xml.Serialization;

namespace NScan.Adapter.ReadingCSharpSolution.ReadingProjects
{
#nullable disable
  [XmlRoot(ElementName = "Reference")]
  public class XmlAssemblyReference
  {
    [XmlElement(ElementName = "HintPath")]
    public string HintPath { get; set; }

    [XmlAttribute(AttributeName = "Include")]
    public string Include { get; set; }
  }
}