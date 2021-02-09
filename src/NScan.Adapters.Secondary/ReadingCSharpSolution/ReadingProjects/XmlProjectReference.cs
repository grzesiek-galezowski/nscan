using System.Xml.Serialization;
using AtmaFileSystem;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects
{
#nullable disable
  [XmlRoot(ElementName = "ProjectReference")]
  public record XmlProjectReference
  {
    [XmlAttribute(AttributeName = "Include")]
    public string Include { get; init; }

    [XmlIgnore]
    public AbsoluteFilePath FullIncludePath { get; set; }
  }
}
