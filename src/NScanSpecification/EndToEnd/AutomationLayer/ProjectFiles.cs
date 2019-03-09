using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TddXt.NScan.ReadingSolution.Ports;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public class ProjectFiles
  {
    private readonly SolutionDir _dir;
    private readonly Dictionary<string, List<XmlSourceCodeFile>> _filesByProject;

    public ProjectFiles(SolutionDir dir)
    {
      _dir = dir;
      _filesByProject = new Dictionary<string, List<XmlSourceCodeFile>>();
    }

    public void AddFilesToProjects()
    {
      foreach (var projectName in _filesByProject.Keys)
      {
        foreach (var sourceCodeFile in _filesByProject[projectName])
        {
          var fileInfo = _dir.PathToFileInProject(DirectoryName(projectName), sourceCodeFile);
          if (!fileInfo.Directory.Exists)
          {
            fileInfo.Directory.Create();
          }

          File.WriteAllText(fileInfo.FullName, StringBodyOf(sourceCodeFile));
        }

      }
    }

    private static string StringBodyOf(XmlSourceCodeFile sourceCodeFile)
    {
      return string.Join(Environment.NewLine, sourceCodeFile.Usings.Select(n => $"using {n};")) + $"namespace {sourceCodeFile.DeclaredNamespaces.Single()}" + " {}";
    }

    public void InitializeForProject(string projectName)
    {
      if (!_filesByProject.ContainsKey(projectName))
      {
        _filesByProject[projectName] = new List<XmlSourceCodeFile>();
      }
    }

    public void Add(string projectName, XmlSourceCodeFile xmlSourceCodeFile)
    {
      _filesByProject[projectName].Add(xmlSourceCodeFile);
    }
  }
}