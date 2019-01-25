using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TddXt.NScan.ReadingSolution.Ports;

namespace TddXt.NScan.Specification.EndToEnd
{
  public class ProjectFiles
  {
    private readonly DirectoryInfo _solutionDir;
    private readonly Dictionary<string, List<XmlSourceCodeFile>> _filesByProject;

    public ProjectFiles(DirectoryInfo solutionDir, Dictionary<string, List<XmlSourceCodeFile>> filesByProject)
    {
      _solutionDir = solutionDir;
      _filesByProject = filesByProject;
    }

    public void AddFilesToProjects()
    {
      foreach (var projectName in _filesByProject.Keys)
      {
        foreach (var sourceCodeFile in _filesByProject[projectName])
        {
          var fullFilePath = Path.Combine(Path.Combine(_solutionDir.FullName, projectName), sourceCodeFile.Name);
          var directoryInfo = new DirectoryInfo(Path.GetDirectoryName(fullFilePath));
          if (!directoryInfo.Exists)
          {
            directoryInfo.Create();
          }

          File.WriteAllText(fullFilePath, StringBodyOf(sourceCodeFile));
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