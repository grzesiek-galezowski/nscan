using System.Collections.Generic;
using System.IO;
using System.Linq;
using TddXt.NScan.App;
using TddXt.NScan.Xml;

namespace TddXt.NScan.CompositionRoot
{
  public interface IWorkspaceModel
  {
    Dictionary<ProjectId, IDotNetProject> LoadProjects();
  }

  public class CsharpWorkspaceModel : IWorkspaceModel
  {
    private readonly ISupport _support;
    private readonly List<XmlProject> _xmlProjects;

    public CsharpWorkspaceModel(ISupport support, List<XmlProject> xmlProjects)
    {
      _support = support;
      _xmlProjects = xmlProjects;
    }

    //todo this is pulled into unit test scope, so write UTs for it...
    private (ProjectId, DotNetStandardProject) CreateProject(XmlProject xmlProject)
    {
      return (new ProjectId(xmlProject.AbsolutePath), new DotNetStandardProject(
        DetermineAssemblyName(xmlProject),
        new ProjectId(xmlProject.AbsolutePath), ProjectReferences(xmlProject).Select(MapToProjectId).ToArray(), 
        _support));
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

    public static IEnumerable<XmlProjectReference> ProjectReferences(XmlProject xmlProject)
    {
      var xmlItemGroups = xmlProject.ItemGroups.Where(ig => ig.ProjectReference.Any()).ToList();
      if (xmlItemGroups.Any())
      {
        return xmlItemGroups.First().ProjectReference;
      }

      return new List<XmlProjectReference>();
    }
  }
}