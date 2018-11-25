using System.Collections.Generic;
using System.IO;
using System.Linq;
using TddXt.NScan.App;
using TddXt.NScan.Domain;
using TddXt.NScan.Xml;

namespace TddXt.NScan.CompositionRoot
{
  public interface IWorkspaceModel
  {
    Dictionary<ProjectId, IDotNetProject> LoadProjects();
  }

  public class CsharpWorkspaceModel : IWorkspaceModel
  {
    private readonly INScanSupport _support;
    private readonly IReadOnlyList<XmlProject> _xmlProjects;

    public CsharpWorkspaceModel(INScanSupport support, IReadOnlyList<XmlProject> xmlProjects)
    {
      _support = support;
      _xmlProjects = xmlProjects;
    }

    //todo this is pulled into unit test scope, so write UTs for it...
    private (ProjectId, DotNetStandardProject) CreateProject(XmlProject xmlProject)
    {
      var assemblyName = DetermineAssemblyName(xmlProject);
      var dotNetStandardProject = new DotNetStandardProject(DetermineRootNamespace(xmlProject, assemblyName), assemblyName,
        new ProjectId(xmlProject.AbsolutePath),
        ProjectReferences(xmlProject).Select(MapToProjectId).ToArray(),
        PackageReferences(xmlProject),
        AssemblyReferences(xmlProject), 
        SourceCodeFiles(xmlProject), _support);
      return (new ProjectId(xmlProject.AbsolutePath), dotNetStandardProject);
    }

    private static List<SourceCodeFile> SourceCodeFiles(XmlProject xmlProject)
    {
      return xmlProject.SourceCodeFiles.Select(scf => new SourceCodeFile(scf)).ToList();
    }

    private static string DetermineRootNamespace(XmlProject xmlProject, string assemblyName)
    {
      return xmlProject.PropertyGroups.First().RootNamespace ?? assemblyName;
    }

    private static string DetermineAssemblyName(XmlProject xmlProject)
    {
      return xmlProject.PropertyGroups.First().AssemblyName ?? Path.GetFileNameWithoutExtension(xmlProject.AbsolutePath);
    }

    private static ProjectId MapToProjectId(XmlProjectReference dto)
    {
      return new ProjectId(dto.Include);
    }

    public Dictionary<ProjectId, IDotNetProject> LoadProjects()
    {
      var projects = new Dictionary<ProjectId, IDotNetProject>();
      foreach (var xmlProject in _xmlProjects)
      {
          var (id, project) = CreateProject(xmlProject);
          projects.Add(id, project);
      }

      return projects;
    }

    private static IReadOnlyList<AssemblyReference> AssemblyReferences(XmlProject xmlProject)
    {
      var xmlItemGroups = xmlProject.ItemGroups.Where(
        ig => ig.AssemblyReferences != null && ig.AssemblyReferences.Any()).ToList();
      if (xmlItemGroups.Any())
      {
        return xmlItemGroups
          .First()
          .AssemblyReferences
          .Select(r => new AssemblyReference(r.Include, r.HintPath)).ToList();
      }

      return new List<AssemblyReference>();
    }


    public static IEnumerable<XmlProjectReference> ProjectReferences(XmlProject xmlProject)
    {
      var xmlItemGroups = xmlProject.ItemGroups
        .Where(ig => ig.ProjectReferences != null && ig.ProjectReferences.Any()).ToList();
      if (xmlItemGroups.Any())
      {
        return xmlItemGroups.First().ProjectReferences;
      }

      return new List<XmlProjectReference>();
    }

    private IReadOnlyList<PackageReference> PackageReferences(XmlProject xmlProject)
    {
      var xmlItemGroups = xmlProject.ItemGroups.Where(
        ig => ig.PackageReferences !=  null && ig.PackageReferences.Any()).ToList();
      if (xmlItemGroups.Any())
      {
        return xmlItemGroups
          .First()
          .PackageReferences
          .Select(r => new PackageReference(r.Include, r.Version)).ToList();
      }

      return new List<PackageReference>();
    }
  }
}