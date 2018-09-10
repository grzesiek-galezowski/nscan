using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MyTool.App;
using MyTool.Xml;
using static MyTool.Maybe;

namespace MyTool.CompositionRoot
{
  internal class CsharpWorkspaceModel
  {
    private readonly ISupport _support;

    public CsharpWorkspaceModel(ISupport support)
    {
      _support = support;
    }

    private XmlProject LoadXmlProjects(string projectFilePath)
    {
      var xmlProject = DeserializeProjectFile(projectFilePath);
      xmlProject.AbsolutePath = projectFilePath;
      NormalizeProjectDependencyPaths(projectFilePath, xmlProject);
      return xmlProject;
    }

    private static (ProjectId, DotNetStandardProject) CreateProject(XmlProject xmlProject)
    {
      return (new ProjectId(xmlProject.AbsolutePath), new DotNetStandardProject(
        xmlProject.PropertyGroups.First().AssemblyName,
        new ProjectId(xmlProject.AbsolutePath),
        ProjectReferences(xmlProject).Select(MapToProjectId).ToArray(), new ConsoleSupport()));
    }

    private static ProjectId MapToProjectId(XmlProjectReference dto)
    {
      return new ProjectId(dto.Include);
    }

    private static XmlProject DeserializeProjectFile(string projectFilePath)
    {
      var serializer = new XmlSerializer(typeof(XmlProject));
      XmlProject result;
      using (var fileStream = new FileStream(projectFilePath, FileMode.Open))
      {
        result = (XmlProject) serializer.Deserialize(fileStream);
      }

      return result;
    }

    private static void NormalizeProjectDependencyPaths(string projectFileAbsolutePath, XmlProject xmlProjectReferences)
    {
      foreach (var projectReference in ProjectReferences(xmlProjectReferences))
      {
        projectReference.Include =
          Path.GetFullPath(Path.Combine(Path.GetDirectoryName(projectFileAbsolutePath), projectReference.Include));
      }
    }

    private static IEnumerable<XmlProjectReference> ProjectReferences(XmlProject xmlProject)
    {
      var xmlItemGroups = xmlProject.ItemGroups.Where(ig => ig.ProjectReference.Any()).ToList();
      if (xmlItemGroups.Any())
      {
        return xmlItemGroups.First().ProjectReference;
      }
      else
      {
        return new List<XmlProjectReference>();
      }
    }

    public Dictionary<ProjectId, IDotNetProject> LoadProjectsPointedToBy(
      IEnumerable<string> projectFilePaths)
    {
      var projects = new Dictionary<ProjectId, IDotNetProject>();

      //todo what about exceptions?
      var xmlProjects = projectFilePaths.Select(path =>
      {
        try
        {
          return Just(LoadXmlProjects(path));
        }
        catch (InvalidOperationException e)
        {
          _support.SkippingProjectBecauseOfError(e, path);
          return Nothing<XmlProject>();
        }
      }).Where(o => o.HasValue).Select(o => o.Value());

        ;

      //todo I/O is separated in flow, now to put into separate classes
      foreach (var xmlProject in xmlProjects)
      {
          var (id, project) = CreateProject(xmlProject);
          projects.Add(id, project);
      }

      return projects;
    }
  }
}