using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MyTool.App;
using MyTool.Xml;

namespace MyTool.CompositionRoot
{
  internal static class CsharpWorkspaceModel
  {
    private static (ProjectId, DotNetStandardProject) LoadProjectFrom(string projectFilePath)
    {
      var xmlProject = DeserializeProjectFile(projectFilePath);
      NormalizeProjectDependencyPaths(projectFilePath, xmlProject);
      return CreateProject(projectFilePath, xmlProject);
    }

    private static (ProjectId, DotNetStandardProject) CreateProject(string projectFilePath, XmlProject xmlProject)
    {
      return (new ProjectId(projectFilePath), new DotNetStandardProject(
        xmlProject.PropertyGroups.First().AssemblyName,
        new ProjectId(projectFilePath),
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

    public static Dictionary<ProjectId, IDotNetProject> LoadProjectsPointedToBy(List<string> projectFilePaths)
    {
      var projects = new Dictionary<ProjectId, IDotNetProject>();
      foreach (var projectFilePath in projectFilePaths)
      {
        try
        {
          var (id, project) = CsharpWorkspaceModel.LoadProjectFrom(projectFilePath);
          projects.Add(id, project);
        }
        catch (InvalidOperationException e)
        {
          Console.WriteLine("Invalid format - skipping " + projectFilePath + " because of " + e);
        }
      }

      return projects;
    }
  }
}