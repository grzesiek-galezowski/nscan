using System.IO;
using TddXt.NScan.Xml;

namespace TddXt.NScan.Domain
{
  public class SourceCodeFile : ISourceCodeFile
  {
    private readonly XmlSourceCodeFile _xmlSourceCodeFile;

    public SourceCodeFile(XmlSourceCodeFile xmlSourceCodeFile)
    {
      _xmlSourceCodeFile = xmlSourceCodeFile;
    }

    public void EvaluateNamespacesCorrectness(IAnalysisReportInProgress report, string ruleDescription)
    {
      if (ExpectedNamespaceOf() != _xmlSourceCodeFile.DeclaredNamespace)
      {
        report.ProjectScopedViolation(ruleDescription,
          _xmlSourceCodeFile.ParentProjectAssemblyName + " has root namespace " +
          _xmlSourceCodeFile.ParentProjectRootNamespace + " but the file "
          + _xmlSourceCodeFile.Name + " has incorrect namespace "
          + _xmlSourceCodeFile.DeclaredNamespace
        );
      }
    }

    private string ExpectedNamespaceOf()
    {
      if (Path.GetFileName(_xmlSourceCodeFile.Name) == _xmlSourceCodeFile.Name)
      {
        return _xmlSourceCodeFile.ParentProjectRootNamespace;
      }
      else
      {
        var fileLocationRelativeToProjectDir = Path.GetDirectoryName(_xmlSourceCodeFile.Name);
        return
          $"{_xmlSourceCodeFile.ParentProjectRootNamespace}.{fileLocationRelativeToProjectDir.Replace(Path.DirectorySeparatorChar, '.')}";
      }
    }
  }
}