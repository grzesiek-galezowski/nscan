using System.IO;
using TddXt.NScan.ReadingSolution.Ports;

namespace TddXt.NScan.Specification.EndToEnd.AutomationLayer
{
  public class SolutionDir
  {
    private readonly string _solutionName;
    private readonly DirectoryInfo _solutionDir;

    public SolutionDir(DirectoryInfo path, string solutionName)
    {
      _solutionDir = path;
      _solutionName = solutionName;
    }

    public string SolutionFilePath()
    {
      return Path.Combine(_solutionDir.FullName, _solutionName + ".sln");
    }

    public string PathToFile(string rulesFileName)
    {
      return Path.Combine(_solutionDir.FullName, rulesFileName);
    }

    public DirectoryInfo PathToFileInProject(string projectName, XmlSourceCodeFile sourceCodeFile)
    {
      var fullFilePath = Path.Combine(Path.Combine(_solutionDir.FullName, projectName), sourceCodeFile.Name);
      var directoryInfo = new DirectoryInfo(Path.GetDirectoryName(fullFilePath));
      return directoryInfo;
    }

    public string FullName()
    {
      return _solutionDir.FullName;
    }

    public string PathToProject(string projectName)
    {
      return Path.Combine(_solutionDir.FullName, projectName);
    }

    public void DeleteWithContent()
    {
      _solutionDir.Delete(true);
    }
  }
}