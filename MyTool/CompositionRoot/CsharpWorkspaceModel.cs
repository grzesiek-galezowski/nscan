using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using MyTool.App;
using MyTool.Xml;

namespace MyTool.CompositionRoot
{
  static internal class CsharpWorkspaceModel
  {
    public static DotNetStandardProject LoadProjectFrom(string projectFilePath)
    {
      XmlSerializer serializer = new XmlSerializer(typeof(XmlProject));
      XmlProject result = null;
      using (FileStream fileStream = new FileStream(projectFilePath, FileMode.Open))
      {
        result = (XmlProject) serializer.Deserialize(fileStream);
      }

      NormalizeProjectDependencyPaths(projectFilePath, result);

      var xmlProjectReferences = ProjectReferences(result);
      var projectMetadata = new DotNetStandardProject(
        result.PropertyGroup.First().AssemblyName,
        new ProjectId(projectFilePath),
        xmlProjectReferences.Select(MapToProjectId()).ToArray());
      return projectMetadata;
    }

    private static Func<XmlProjectReference, ProjectId> MapToProjectId()
    {
      return dto => new ProjectId(dto.Include);
    }

    private static void NormalizeProjectDependencyPaths(string projectFileAbsolutePath, XmlProject xmlProjectReferences)
    {
      foreach (var projectReference in ProjectReferences(xmlProjectReferences))
      {
        projectReference.Include =
          Path.GetFullPath(Path.Combine(Path.GetDirectoryName(projectFileAbsolutePath), projectReference.Include));
      }
    }

    private static List<XmlProjectReference> ProjectReferences(XmlProject xmlProject)
    {
      return xmlProject.ItemGroup.First(ig => ig.ProjectReference.Any()).ProjectReference;
    }
  }
}