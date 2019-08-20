using System.Collections.Generic;
using System.Linq;
using AnyClone;
using AtmaFileSystem;
using Functional.Maybe;
using NScan.SharedKernel.ReadingSolution.Ports;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScan.SharedKernel.ReadingSolution.Lib
{
  public interface IXmlProjectDataAccess
  {
    string DetermineAssemblyName();
    ProjectId Id();
    IEnumerable<XmlPackageReference> XmlPackageReferences();
    IEnumerable<XmlAssemblyReference> XmlAssemblyReferences();
    IEnumerable<XmlProjectReference> ProjectReferences();
    IEnumerable<SourceCodeFileDto> SourceCodeFiles();
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
      var references = _xmlProject.ItemGroups
        .FirstMaybe(ig => ig.AssemblyReferences != null && ig.AssemblyReferences.Any())
        .Select(ig => ig.AssemblyReferences.ToList())
        .OrElse(() => new List<XmlAssemblyReference>());
      return references;
    }

    public string DetermineAssemblyName()
    {
      return _xmlProject.PropertyGroups
        .FirstMaybe(pg => pg.AssemblyName != null)
        .Select(pg => pg.AssemblyName)
        .OrElse(() => _xmlProject.AbsolutePath.FileName().WithoutExtension().ToString());
    }

    public IEnumerable<XmlProjectReference> ProjectReferences()
    {
        return _xmlProject.ItemGroups
          .FirstMaybe(ig => ig.ProjectReferences != null && ig.ProjectReferences.Any())
          .Select(ig => ig.ProjectReferences.ToList())
          .OrElse(() => new List<XmlProjectReference>());
    }

    public ProjectId Id()
    {
      return new ProjectId(_xmlProject.AbsolutePath.ToString());
    }

    public IEnumerable<SourceCodeFileDto> SourceCodeFiles()
    {
      return _xmlProject.SourceCodeFiles!;
    }

    public string TargetFramework()
    {
      return _xmlProject.PropertyGroups.First(pg => pg.TargetFramework != null).TargetFramework;
    }

    public string RootNamespace()
    {
      return _xmlProject.PropertyGroups
        .FirstMaybe(p => p.RootNamespace != null)
        .Select(pg => pg.RootNamespace)
        .OrElse(() => _xmlProject.AbsolutePath.FileName().WithoutExtension().ToString());
    }

    public void AddFile(SourceCodeFileDto xmlSourceCodeFile)
    {
      _xmlProject.SourceCodeFiles.Add(xmlSourceCodeFile);
    }

    public AbsoluteDirectoryPath GetParentDirectoryName()
    {
      return _xmlProject.AbsolutePath.ParentDirectory();
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
      return _xmlProject.Clone();
    }
  }


}