using System.IO;
using System.Linq;
using TddXt.NScan.ReadingSolution.Ports;

namespace TddXt.NScan.Domain
{
  public class SourceCodeFile : ISourceCodeFile
  {
    private readonly XmlSourceCodeFile _xmlSourceCodeFile;
    private readonly IRuleViolationFactory _ruleViolationFactory;

    public SourceCodeFile(XmlSourceCodeFile xmlSourceCodeFile, IRuleViolationFactory ruleViolationFactory)
    {
      _xmlSourceCodeFile = xmlSourceCodeFile;
      _ruleViolationFactory = ruleViolationFactory;
    }

    public void EvaluateNamespacesCorrectness(IAnalysisReportInProgress report, string ruleDescription)
    {
      if (_xmlSourceCodeFile.DeclaredNamespaces.Count == 0)
      {
        report.Add(_ruleViolationFactory.ProjectScopedRuleViolation(ruleDescription, 
          ViolationDescription("has no namespace declared"))
        );
      }
      else if (_xmlSourceCodeFile.DeclaredNamespaces.Count > 1)
      {
        report.Add(_ruleViolationFactory.ProjectScopedRuleViolation(ruleDescription, 
          ViolationDescription($"declares multiple namespaces: {NamespacesString()}")));
      }
      else if (!_xmlSourceCodeFile.DeclaredNamespaces.Contains(CorrectNamespace()))
      {
        report.Add(_ruleViolationFactory.ProjectScopedRuleViolation(ruleDescription, 
          ViolationDescription($"has incorrect namespace {_xmlSourceCodeFile.DeclaredNamespaces.Single()}"))
        );
      }
    }

    public void AddNamespaceMappingTo(INamespacesDependenciesCache namespacesDependenciesCache)
    {
      foreach (var declaredNamespace in _xmlSourceCodeFile.DeclaredNamespaces)
      {
        foreach (var @using in _xmlSourceCodeFile.Usings)
        {
          namespacesDependenciesCache.AddMapping(declaredNamespace, @using);
        }
      }
    }

    private string NamespacesString()
    {
      return string.Join(", ", _xmlSourceCodeFile.DeclaredNamespaces);
    }

    private string ViolationDescription(string reason)
    {
      return _xmlSourceCodeFile.ParentProjectAssemblyName + " has root namespace " +
             _xmlSourceCodeFile.ParentProjectRootNamespace + " but the file "
             + _xmlSourceCodeFile.Name + " " + reason;
    }

    private string CorrectNamespace()
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