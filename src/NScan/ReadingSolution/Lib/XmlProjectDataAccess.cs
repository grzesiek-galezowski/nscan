using System.Collections.Generic;
using System.IO;
using System.Linq;
using Functional.Maybe;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingSolution.Ports;

namespace TddXt.NScan.ReadingSolution.Lib
{
  public interface IXmlProjectDataAccess
  {
    void NormalizeProjectDependencyPaths(string projectFileAbsolutePath);
    string DetermineAssemblyName();
    ProjectId Id();
    IEnumerable<XmlPackageReference> XmlPackageReferences();
    IEnumerable<XmlAssemblyReference> XmlAssemblyReferences();
    IEnumerable<XmlProjectReference> ProjectReferences();
    IEnumerable<XmlSourceCodeFile> SourceCodeFiles();
  }

  public class XmlProjectDataAccess : IXmlProjectDataAccess
  {
    private readonly XmlProject _xmlProject;

    public XmlProjectDataAccess(XmlProject xmlProject)
    {
      _xmlProject = xmlProject;
    }

    public IEnumerable<XmlPackageReference> XmlPackageReferences()
    {
      var xmlItemGroups = _xmlProject.ItemGroups.Where(
        ig => ig.PackageReferences != null && ig.PackageReferences.Any()).ToList();

      var references = xmlItemGroups
        .FirstMaybe().Select(pr => pr.PackageReferences.ToList()).OrElse(() => new List<XmlPackageReference>());
      return references;
    }

    public IEnumerable<XmlAssemblyReference> XmlAssemblyReferences()
    {
      var xmlItemGroups = _xmlProject.ItemGroups.Where(
        ig => ig.AssemblyReferences != null && ig.AssemblyReferences.Any()).ToList();


      var xmlAssemblyReferences = xmlItemGroups
        .FirstMaybe()
        .Select(ig => ig.AssemblyReferences)
        .OrElse(() => new List<XmlAssemblyReference>());
      return xmlAssemblyReferences;
    }

    public string DetermineAssemblyName()
    {
      return _xmlProject.PropertyGroups.First().AssemblyName ?? Path.GetFileNameWithoutExtension(_xmlProject.AbsolutePath);
    }

    public IEnumerable<XmlProjectReference> ProjectReferences()
    {
      var xmlItemGroups = _xmlProject.ItemGroups
        .Where(ig => ig.ProjectReferences != null && ig.ProjectReferences.Any()).ToList();
      if (xmlItemGroups.Any())
      {
        return xmlItemGroups.First().ProjectReferences;
      }

      return new List<XmlProjectReference>();
    }

    public ProjectId Id()
    {
      return new ProjectId(_xmlProject.AbsolutePath);
    }

    public IEnumerable<XmlSourceCodeFile> SourceCodeFiles()
    {
      return _xmlProject.SourceCodeFiles;
    }

    public string RootNamespace()
    {
      return _xmlProject.PropertyGroups.First().RootNamespace;
    }

    public void AddFile(XmlSourceCodeFile xmlSourceCodeFile)
    {
      _xmlProject.SourceCodeFiles.Add(xmlSourceCodeFile);
    }

    public string GetParentDirectoryName()
    {
      return Path.GetDirectoryName(_xmlProject.AbsolutePath);
    }

    public void NormalizeProjectAssemblyName()
    {
      if (_xmlProject.PropertyGroups.All(g => g.AssemblyName == null))
      {
        _xmlProject.PropertyGroups.First().AssemblyName
          = Path.GetFileNameWithoutExtension(
            Path.GetFileName(_xmlProject.AbsolutePath));
      }
    }

    public void NormalizeProjectRootNamespace()
    {
      if (_xmlProject.PropertyGroups.All(g => g.RootNamespace == null))
      {
        _xmlProject.PropertyGroups.First().RootNamespace
          = Path.GetFileNameWithoutExtension(
            Path.GetFileName(_xmlProject.AbsolutePath));
      }
    }

    public void NormalizeProjectDependencyPaths(string projectFileAbsolutePath)
    {
      foreach (var projectReference in ProjectReferences())
      {
        projectReference.Include =
          Path.GetFullPath(Path.Combine(Path.GetDirectoryName(projectFileAbsolutePath), projectReference.Include));
      }
    }

    public void SetAbsolutePath(string projectFilePath)
    {
      _xmlProject.AbsolutePath = projectFilePath;
    }

    public XmlProject ToXmlProject()
    {
      return _xmlProject; //bug clone this
    }
  }
}