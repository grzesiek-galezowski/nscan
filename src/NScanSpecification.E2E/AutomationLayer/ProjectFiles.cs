using System.Collections.Generic;
using System.IO;
using NScan.SharedKernel.ReadingSolution.Ports;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScanSpecification.E2E.AutomationLayer
{
  public class ProjectFiles
  {
    private readonly SolutionDir _dir;
    private readonly Dictionary<string, List<SourceCodeFileDto>> _filesByProject;

    public ProjectFiles(SolutionDir dir)
    {
      _dir = dir;
      _filesByProject = new Dictionary<string, List<SourceCodeFileDto>>();
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

          var generateFrom = SourceCodeFileText.GenerateFrom(sourceCodeFile);
          File.WriteAllText(fileInfo.FullName, generateFrom);
        }
      }
    }

    public void InitializeForProject(string projectName)
    {
      if (!_filesByProject.ContainsKey(projectName))
      {
        _filesByProject[projectName] = new List<SourceCodeFileDto>();
      }
    }

    public void Add(string projectName, SourceCodeFileDto xmlSourceCodeFile)
    {
      _filesByProject[projectName].Add(xmlSourceCodeFile);
    }
  }
}