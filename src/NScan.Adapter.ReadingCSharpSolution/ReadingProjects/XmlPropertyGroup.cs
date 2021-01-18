using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;

namespace NScan.Adapter.ReadingCSharpSolution.ReadingProjects
{
  [XmlRoot(ElementName = "PropertyGroup")]
  public record XmlPropertyGroup
  {
    [XmlElement(ElementName = nameof(TargetFramework))]
    public string? TargetFramework { get; init; }

    [XmlElement(ElementName = nameof(AssemblyName))]
    public string? AssemblyName { get; init; }

    [XmlElement(ElementName = nameof(RootNamespace))]
    public string? RootNamespace { get; init; }

    [XmlElement(ElementName = nameof(SignAssembly))]
    public string? SignAssembly { get; init; }

    [XmlElement(ElementName = nameof(AssemblyOriginatorKeyFile))]
    public string? AssemblyOriginatorKeyFile { get; init; }

    [XmlElement(ElementName = nameof(DelaySign))]
    public string? DelaySign { get; init; }

    [XmlElement(ElementName = nameof(OutputType))]
    public string? OutputType { get; init; }

    [XmlElement(ElementName = nameof(OutputPath))]
    public string? OutputPath { get; init; }

    [XmlAttribute(AttributeName = nameof(Condition))]
    public string? Condition { get; init; }

    [XmlElement(ElementName = nameof(PackAsTool))]
    public string? PackAsTool { get; init; }

    [XmlElement(ElementName = nameof(ToolCommandName))]
    public string? ToolCommandName { get; init; }

    [XmlElement(ElementName = nameof(LangVersion))]
    public string? LangVersion { get; init; }

    [XmlElement(ElementName = nameof(Nullable))]
    public string? Nullable { get; init; }

    [XmlElement(ElementName = nameof(WarningsAsErrors))]
    public string? WarningsAsErrors { get; init; }

    [XmlElement(ElementName = nameof(PackageRequireLicenseAcceptance))]
    public string? PackageRequireLicenseAcceptance { get; init; }

    [XmlElement(ElementName = nameof(PackageId))]
    public string? PackageId { get; init; }

    [XmlElement(ElementName = nameof(Authors))]
    public string? Authors { get; init; }

    [XmlElement(ElementName = nameof(Product))]
    public string? Product { get; init; }

    [XmlElement(ElementName = nameof(Description))]
    public string? Description { get; init; }

    [XmlElement(ElementName = nameof(PackageLicenseFile))]
    public string? PackageLicenseFile { get; init; }

    [XmlElement(ElementName = nameof(PackageProjectUrl))]
    public string? PackageProjectUrl { get; init; }

    [XmlElement(ElementName = nameof(PackageIconUrl))]
    public string? PackageIconUrl { get; init; }

    [XmlElement(ElementName = nameof(RepositoryUrl))]
    public string? RepositoryUrl { get; init; }

    [XmlElement(ElementName = nameof(RepositoryType))]
    public string? RepositoryType { get; init; }

    [XmlElement(ElementName = nameof(PackageTags))]
    public string? PackageTags { get; init; }

    [XmlElement(ElementName = nameof(PackageReleaseNotes))]
    public string? PackageReleaseNotes { get; init; }

    [XmlElement(ElementName = nameof(GeneratePackageOnBuild))]
    public string? GeneratePackageOnBuild { get; init; }

    [XmlIgnore] 
    public ImmutableDictionary<string, string> Properties => CollectProperties();

    private ImmutableDictionary<string, string> CollectProperties()
    {
      var propertyInfos = this.GetType().GetProperties(
          BindingFlags.Public | BindingFlags.Instance)
        .Where(p => p.IsDefined(typeof(XmlElementAttribute)))
        .Where(p => p.GetValue(this) != null)
        .Select(p => (name : p.Name, value : p.GetValue(this)))
        .ToImmutableDictionary(p => p.name, p => p.value.ToString());
      return propertyInfos;
    }
  }

}
