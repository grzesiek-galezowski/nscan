using System.IO;
using System.Linq;
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
      //TODO some duplication
      if (_xmlSourceCodeFile.DeclaredNamespaces.Count == 0)
      {
        report.ProjectScopedViolation(ruleDescription,
          _xmlSourceCodeFile.ParentProjectAssemblyName + " has root namespace " +
          _xmlSourceCodeFile.ParentProjectRootNamespace + " but the file "
          + _xmlSourceCodeFile.Name + " has no namespace declared"
        );
      }
      else if (_xmlSourceCodeFile.DeclaredNamespaces.Count > 1)
      {
        report.ProjectScopedViolation(ruleDescription,
          _xmlSourceCodeFile.ParentProjectAssemblyName + " has root namespace " +
          _xmlSourceCodeFile.ParentProjectRootNamespace + " but the file "
          + _xmlSourceCodeFile.Name + $" declares multiple namespaces: {string.Join(", ", _xmlSourceCodeFile.DeclaredNamespaces)}"
        );
      }
      else if (!_xmlSourceCodeFile.DeclaredNamespaces.Contains(ExpectedNamespace()))
      {
        report.ProjectScopedViolation(ruleDescription,
          _xmlSourceCodeFile.ParentProjectAssemblyName + " has root namespace " +
          _xmlSourceCodeFile.ParentProjectRootNamespace + " but the file "
          + _xmlSourceCodeFile.Name + " has incorrect namespace "
          + _xmlSourceCodeFile.DeclaredNamespaces.Single()
        );
      }
    }

    private string ExpectedNamespace()
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