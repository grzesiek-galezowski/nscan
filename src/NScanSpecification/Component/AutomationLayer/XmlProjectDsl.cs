using System.Collections.Generic;
using System.Linq;
using Castle.Components.DictionaryAdapter;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Xml;

namespace TddXt.NScan.Specification.Component.AutomationLayer
{
  public class XmlProjectDsl
  {
    private readonly XmlProject _xmlProject;

    public XmlProjectDsl(XmlProject xmlProject)
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
        HintPath = AnyRoot.Root.Any.String(),
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
      return names.Select(n => new XmlProjectReference()
      {
        Include = NScanDriver.AbsolutePathFor(n)
      }).ToList();
    }


    public XmlProjectDsl WithFile(string fileName, string @namespace)
    {
      _xmlProject.SourceCodeFiles.Add(new XmlSourceCodeFile(
        fileName, 
        new List<string> { @namespace } ,
        _xmlProject.PropertyGroups.First().RootNamespace,
        _xmlProject.PropertyGroups.First().AssemblyName, 
        new List<string>(/* bug */)));
      return this;
    }

    public XmlProjectDsl WithRootNamespace(string rootNamespace)
    {
      _xmlProject.PropertyGroups.First().RootNamespace = rootNamespace;
      return this;
    }
  }
}