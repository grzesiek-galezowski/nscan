using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.ReadingSolution.Ports;
using TddXt.NScan.Specification.AutomationLayer;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace TddXt.NScan.Specification.Component.AutomationLayer
{
  public class XmlProjectBuilder
  {
    private readonly XmlProject _xmlProject;

    public XmlProjectBuilder(XmlProject xmlProject)
    {
      _xmlProject = xmlProject;
      _xmlProject.ItemGroups = _xmlProject.ItemGroups ?? new List<XmlItemGroup>();
    }

    public void WithReferences(params string[] names)
    {
      _xmlProject.ItemGroups.Add(
        new XmlItemGroup() {ProjectReferences = ProjectReferencesFrom(names)}
      );
    }

    public void WithPackages(params string[] packageNames)
    {
      _xmlProject.ItemGroups.Add(
        new XmlItemGroup() {PackageReferences = PackageReferencesFrom(packageNames)}
      );
    }

    public void WithAssemblyReferences(params string[] assemblyNames)
    {
      _xmlProject.ItemGroups.Add(
        new XmlItemGroup() { AssemblyReferences = AssemblyReferencesFrom(assemblyNames) }
      );
    }

    private List<XmlAssemblyReference> AssemblyReferencesFrom(string[] assemblyNames)
    {
      return assemblyNames.Select(name => new XmlAssemblyReference()
      {
        HintPath = Root.Any.String(),
        Include = name
      }).ToList();
    }


    private List<XmlPackageReference> PackageReferencesFrom(string[] packageNames)
    {
      return packageNames.Select(name => new XmlPackageReference()
      {
        Include = name,
        Version = "0.0.0"
      }).ToList();
    }

    private static List<XmlProjectReference> ProjectReferencesFrom(string[] names)
    {
      return names.Select(n => new XmlProjectReference
      {
        FullIncludePath = AbsolutePathTo(n)
      }).ToList();
    }


    public XmlProjectBuilder With(XmlSourceCodeFileBuilder xmlSourceCodeFileBuilder)
    {
      var xmlSourceCodeFile = xmlSourceCodeFileBuilder
        .BuildWith(
          _xmlProject.PropertyGroups.First().RootNamespace,
          _xmlProject.PropertyGroups.First().AssemblyName
        );

      _xmlProject.SourceCodeFiles.Add(xmlSourceCodeFile);
      return this;
    }

    public XmlProjectBuilder WithRootNamespace(string rootNamespace)
    {
      _xmlProject.PropertyGroups.First().RootNamespace = rootNamespace;
      return this;
    }

    public XmlProject Build()
    {
      return _xmlProject;
    }

    private static AbsoluteFilePath AbsolutePathTo(string assemblyName)
    {
      return AbsoluteFilePath(@"C:\" + assemblyName + ".cs");
    }

    public static XmlProjectBuilder WithAssemblyName(string assemblyName)
    {
      return new XmlProjectBuilder(new XmlProject
      {
        AbsolutePath = AbsolutePathTo(assemblyName),
        PropertyGroups = new List<XmlPropertyGroup>
        {
          new XmlPropertyGroup
          {
            AssemblyName = assemblyName
          }
        },
        ItemGroups = new List<XmlItemGroup>()
      });
    }
  }
}