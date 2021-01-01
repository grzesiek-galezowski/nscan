using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AtmaFileSystem;
using Functional.Maybe;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingSolution.Ports;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScan.Adapter.ReadingCSharpSolution.ReadingProjects
{
  public class XmlProjectDataAccess
  {
    private XmlProject _xmlProject;

    public XmlProjectDataAccess(XmlProject xmlProject)
    {
      _xmlProject = xmlProject;
    }

    private IEnumerable<XmlPackageReference> XmlPackageReferences()
    {
      var xmlItemGroups = _xmlProject.ItemGroups.Where(
        ig => ig.PackageReferences != null && ig.PackageReferences.Any()).ToList();

      var references = xmlItemGroups
        .FirstMaybe()
        .Select(pr => pr.PackageReferences.ToList())
        .OrElse(() => new List<XmlPackageReference>());
      return references;
    }

    private IEnumerable<XmlAssemblyReference> XmlAssemblyReferences()
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

    private IEnumerable<XmlProjectReference> ProjectReferences()
    {
        return _xmlProject.ItemGroups
          .FirstMaybe(ig => ig.ProjectReferences != null && ig.ProjectReferences.Any())
          .Select(ig => ig.ProjectReferences.ToList())
          .OrElse(() => new List<XmlProjectReference>());
    }

    private ProjectId Id()
    {
      return new(_xmlProject.AbsolutePath.ToString());
    }

    private ImmutableList<SourceCodeFileDto> SourceCodeFiles()
    {
      return _xmlProject.SourceCodeFiles!.ToImmutableList();
    }

    private string TargetFramework()
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
      _xmlProject = _xmlProject with { AbsolutePath = projectFilePath };
    }

    public CsharpProjectDto BuildCsharpProjectDto()
    {
      return new(
        Id(), 
        DetermineAssemblyName(), 
        TargetFramework(), 
        SourceCodeFiles(), 
        XmlPackageReferences()
        .Select(r => new PackageReference(r.Include, r.Version)).ToImmutableList(), 
        XmlAssemblyReferences()
        .Select(r => new AssemblyReference(r.Include, r.HintPath)).ToImmutableList(), 
        ProjectReferences()
        .Select(dto => new ProjectId(dto.FullIncludePath.ToString())).ToImmutableList());
    }
  }


}
