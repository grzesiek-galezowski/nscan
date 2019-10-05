using System.Collections.Generic;
using System.IO;
using System.Linq;
using AtmaFileSystem;
using NScan.Lib;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;

namespace NScan.Domain.Root
{
  public class SourceCodeFile : ISourceCodeFile
  {
    private readonly IProjectScopedRuleViolationFactory _ruleViolationFactory;
    private readonly IReadOnlyList<string> _declaredNamespaces;
    private readonly string _parentProjectAssemblyName;
    private readonly string _parentProjectRootNamespace;
    private readonly RelativeFilePath _pathRelativeToProjectRoot;
    private readonly IReadOnlyList<string> _usings;
    private readonly ICSharpClass[] _classes;

    public SourceCodeFile(IProjectScopedRuleViolationFactory ruleViolationFactory,
      IReadOnlyList<string> declaredNamespaces,
      string parentProjectAssemblyName,
      string parentProjectRootNamespace,
      RelativeFilePath pathRelativeToProjectRoot,
      IReadOnlyList<string> usings, 
      ICSharpClass[] classes)
    {
      _ruleViolationFactory = ruleViolationFactory;
      _declaredNamespaces = declaredNamespaces;
      _parentProjectAssemblyName = parentProjectAssemblyName;
      _parentProjectRootNamespace = parentProjectRootNamespace;
      _pathRelativeToProjectRoot = pathRelativeToProjectRoot;
      _usings = usings;
      _classes = classes;
    }

    public void EvaluateNamespacesCorrectness(IAnalysisReportInProgress report, string ruleDescription)
    {
      if (_declaredNamespaces.Count == 0)
      {
        report.Add(_ruleViolationFactory.ProjectScopedRuleViolation(ruleDescription, 
          ViolationDescription("has no namespace declared"))
        );
      }
      else if (_declaredNamespaces.Count > 1)
      {
        report.Add(_ruleViolationFactory.ProjectScopedRuleViolation(ruleDescription, 
          ViolationDescription($"declares multiple namespaces: {NamespacesString()}")));
      }
      else if (!_declaredNamespaces.Contains(CorrectNamespace()))
      {
        report.Add(_ruleViolationFactory.ProjectScopedRuleViolation(ruleDescription, 
          ViolationDescription($"has incorrect namespace {_declaredNamespaces.Single()}"))
        );
      }
    }

    public void EvaluateMethodsHavingCorrectAttributes(
      IAnalysisReportInProgress report, 
      Pattern classNameInclusionPattern,
      Pattern methodNameInclusionPattern, 
      string ruleDescription)
    {
      foreach (var cSharpClass in _classes)
      {
        if (cSharpClass.NameMatches(classNameInclusionPattern))
        {
          cSharpClass.EvaluateDecorationWithAttributes(report, methodNameInclusionPattern, ruleDescription);
        }
      }
    }

    public void AddNamespaceMappingTo(INamespacesDependenciesCache namespacesDependenciesCache)
    {
      foreach (var declaredNamespace in _declaredNamespaces)
      {
        foreach (var @using in _usings)
        {
          namespacesDependenciesCache.AddMapping(declaredNamespace, @using);
        }
      }
    }

    private string NamespacesString()
    {
      return string.Join(", ", _declaredNamespaces);
    }

    private string ViolationDescription(string reason)
    {
      return _parentProjectAssemblyName + " has root namespace " +
             _parentProjectRootNamespace + " but the file "
             + _pathRelativeToProjectRoot + " " + reason;
    }

    private string CorrectNamespace()
    {
      if (!_pathRelativeToProjectRoot.ParentDirectory().HasValue)
      {
        return _parentProjectRootNamespace;
      }
      else
      {
        var fileLocationRelativeToProjectDir = _pathRelativeToProjectRoot.ParentDirectory();
        return
          $"{_parentProjectRootNamespace}.{fileLocationRelativeToProjectDir.ToString().Replace(Path.DirectorySeparatorChar, '.')}";
      }
    }

    public override string ToString()
    {
      return _pathRelativeToProjectRoot.ToString();
    }
  }
}