using System.IO;
using AtmaFileSystem;
using AtmaFileSystem.IO;
using NScan.SharedKernel.ReadingSolution.Ports;
using static AtmaFileSystem.AtmaFileSystemPaths;
using AbsoluteDirectoryPath = AtmaFileSystem.AbsoluteDirectoryPath;
using DirectoryName = AtmaFileSystem.DirectoryName;
using FileName = AtmaFileSystem.FileName;

namespace NScanSpecification.E2E.AutomationLayer;

public class SolutionDir
{
  private readonly string _solutionName;
  private readonly DirectoryInfo _solutionDir;
  private readonly AbsoluteDirectoryPath _absoluteSolutionDirectoryPath;

  public SolutionDir(DirectoryInfo path, string solutionName)
  {
    _solutionDir = path;
    _solutionName = solutionName;
    _absoluteSolutionDirectoryPath = AbsoluteDirectoryPath(_solutionDir.FullName);
  }

  public AbsoluteFilePath SolutionFilePath()
  {
    // .NET 10+ can create either .sln or .slnx files
    // Check which one actually exists
    var classicSln = _absoluteSolutionDirectoryPath + FileName(_solutionName + ".sln");
    if (classicSln.Exists())
    {
      return classicSln;
    }

    var xmlSln = _absoluteSolutionDirectoryPath + FileName(_solutionName + ".slnx");
    if (xmlSln.Exists())
    {
      return xmlSln;
    }

    // Default to .sln if neither exists (for creation scenario)
    return classicSln;
  }

  public AbsoluteFilePath PathToFile(FileName rulesFileName)
  {
    return _absoluteSolutionDirectoryPath + rulesFileName;
  }

  public FileInfo PathToFileInProject(DirectoryName projectDirectoryName, SourceCodeFileDto sourceCodeFile)
  {
    var fullFilePath = _absoluteSolutionDirectoryPath + projectDirectoryName + sourceCodeFile.PathRelativeToProjectRoot;
    return fullFilePath.Info();
  }

  public AbsoluteDirectoryPath FullName()
  {
    return _absoluteSolutionDirectoryPath;
  }

  public void DeleteWithContent()
  {
    _solutionDir.Delete(true);
  }

  public AbsoluteDirectoryPath PathToProject(string projectName)
  {
    return _absoluteSolutionDirectoryPath + DirectoryName(projectName);
  }

  public void AssertExists()
  {
    if (!_absoluteSolutionDirectoryPath.Exists())
    {
      throw new SolutionPathDoesNotExistException(_absoluteSolutionDirectoryPath);
    }
  }
}

public class SolutionPathDoesNotExistException(AbsoluteDirectoryPath absoluteSolutionDirectoryPath)
  : Exception($"Solution path {absoluteSolutionDirectoryPath} does not exist");
