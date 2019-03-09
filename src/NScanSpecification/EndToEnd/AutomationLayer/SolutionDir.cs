using System.IO;
using AtmaFileSystem;
using TddXt.NScan.ReadingSolution.Ports;
using static AtmaFileSystem.AtmaFileSystemPaths;
using AbsoluteDirectoryPath = AtmaFileSystem.AbsoluteDirectoryPath;
using DirectoryName = AtmaFileSystem.DirectoryName;
using FileName = AtmaFileSystem.FileName;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
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
      return _absoluteSolutionDirectoryPath + FileName(_solutionName + ".sln");
    }

    public AbsoluteFilePath PathToFile(FileName rulesFileName)
    {
      return _absoluteSolutionDirectoryPath + rulesFileName;
    }

    public FileInfo PathToFileInProject(DirectoryName projectDirectoryName, XmlSourceCodeFile sourceCodeFile)
    {
      var fullFilePath = _absoluteSolutionDirectoryPath + projectDirectoryName + RelativeFilePath(sourceCodeFile.Name);
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
  }
}