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
    public static DotNetStandardProject LoadProjectFrom(string projectFilePath)
    {
      var xmlProject = DeserializeProjectFile(projectFilePath);
      NormalizeProjectDependencyPaths(projectFilePath, xmlProject);
      return CreateProject(projectFilePath, xmlProject);
    }

    private static DotNetStandardProject CreateProject(string projectFilePath, XmlProject xmlProject)
    {
      return new DotNetStandardProject(
        xmlProject.PropertyGroups.First().AssemblyName,
        new ProjectId(projectFilePath),
        ProjectReferences(xmlProject).Select(MapToProjectId).ToArray());
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
  }
}