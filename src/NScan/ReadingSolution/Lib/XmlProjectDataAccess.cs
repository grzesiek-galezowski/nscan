using System.Collections.Generic;
using System.Linq;
using AtmaFileSystem;
using Functional.Maybe;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingSolution.Ports;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace TddXt.NScan.ReadingSolution.Lib
{
  public interface IXmlProjectDataAccess
  {
    void NormalizeProjectDependencyPaths(AbsoluteFilePath projectFileAbsolutePath);
    string DetermineAssemblyName();
    ProjectId Id();
    IEnumerable<XmlPackageReference> XmlPackageReferences();
    IEnumerable<XmlAssemblyReference> XmlAssemblyReferences();
    IEnumerable<XmlProjectReference> ProjectReferences();
    IEnumerable<XmlSourceCodeFile> SourceCodeFiles();
    string TargetFramework();
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
        .FirstMaybe()
        .Select(pr => pr.PackageReferences.ToList())
        .OrElse(() => new List<XmlPackageReference>());
      return references;
    }

    public IEnumerable<XmlAssemblyReference> XmlAssemblyReferences()
    {
      var xmlItemGroups = _xmlProject.ItemGroups.Where(
        ig => ig.AssemblyReferences != null && ig.AssemblyReferences.Any()).ToList();

      var references = xmlItemGroups
        .FirstMaybe()
        .Select(ig => ig.AssemblyReferences.ToList())
        .OrElse(() => new List<XmlAssemblyReference>());
      return references;
    }

    public string DetermineAssemblyName()
    {
      return _xmlProject.PropertyGroups.First().AssemblyName ?? _xmlProject.AbsolutePath.FileName().WithoutExtension().ToString();
    }

    public IEnumerable<XmlProjectReference> ProjectReferences()
    {
      var xmlItemGroups = _xmlProject.ItemGroups
        .Where(ig => ig.ProjectReferences != null && ig.ProjectReferences.Any()).ToList();
      if (xmlItemGroups.Any())
      {
        return xmlItemGroups.First().ProjectReferences.ToList();
      }

      return new List<XmlProjectReference>();
    }

    public ProjectId Id()
    {
      return new ProjectId(_xmlProject.AbsolutePath.ToString());
    }

    public IEnumerable<XmlSourceCodeFile> SourceCodeFiles()
    {
      return _xmlProject.SourceCodeFiles!;
    }

    public string TargetFramework()
    {
      return _xmlProject.PropertyGroups.First(pg => pg.TargetFramework != null).TargetFramework;
    }

    public string RootNamespace()
    {
      return _xmlProject.PropertyGroups.First().RootNamespace!;
    }

    public void AddFile(XmlSourceCodeFile xmlSourceCodeFile)
    {
      _xmlProject.SourceCodeFiles.Add(xmlSourceCodeFile);
    }

    public AbsoluteDirectoryPath GetParentDirectoryName()
    {
      return _xmlProject.AbsolutePath.ParentDirectory();
    }

    public void NormalizeProjectAssemblyName()
    {
      if (_xmlProject.PropertyGroups.All(g => g.AssemblyName == null))
      {
        _xmlProject.PropertyGroups.First().AssemblyName
          = _xmlProject.AbsolutePath.FileName().WithoutExtension().ToString();
      }
    }

    public void NormalizeProjectRootNamespace()
    {
      if (_xmlProject.PropertyGroups.All(g => g.RootNamespace == null))
      {
        _xmlProject.PropertyGroups.First().RootNamespace
          = _xmlProject.AbsolutePath.FileName().WithoutExtension().ToString();
      }
    }

    public void NormalizeProjectDependencyPaths(AbsoluteFilePath projectFileAbsolutePath)
    {
      foreach (var projectReference in ProjectReferences())
      {
        projectReference.FullIncludePath = 
          projectFileAbsolutePath.ParentDirectory() + RelativeFilePath(projectReference.Include);
      }
    }

    public void SetAbsolutePath(AbsoluteFilePath projectFilePath)
    {
      _xmlProject.AbsolutePath = projectFilePath;
    }

    public XmlProject ToXmlProject()
    {
      return _xmlProject; //bug clone this
    }
  }
}