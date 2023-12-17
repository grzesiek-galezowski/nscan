using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NScan.SharedKernel.ReadingSolution.Ports;
using Core.NullableReferenceTypesExtensions;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScanSpecification.E2E.AutomationLayer;

public class ProjectFiles(SolutionDir dir)
{
  private readonly Dictionary<string, List<SourceCodeFileDto>> _filesByProject = new();

  public async Task AddFilesToProjects(CancellationToken cancellationToken)
  {
    foreach (var projectName in _filesByProject.Keys)
    {
      foreach (var sourceCodeFile in _filesByProject[projectName])
      {
        var fileInfo = dir.PathToFileInProject(DirectoryName(projectName), sourceCodeFile);
        var projectDirectoryInfo = fileInfo.Directory.OrThrow();
        if (!projectDirectoryInfo.Exists)
        {
          projectDirectoryInfo.Create();
        }

        var generateFrom = SourceCodeFileText.GenerateFrom(sourceCodeFile);
        await File.WriteAllTextAsync(fileInfo.FullName, generateFrom, cancellationToken: cancellationToken);
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
