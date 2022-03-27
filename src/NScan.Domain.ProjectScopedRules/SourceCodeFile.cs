using System.Collections.Generic;
using System.IO;
using System.Linq;
using AtmaFileSystem;
using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public class SourceCodeFile : ISourceCodeFileInNamespace
{
  private readonly IProjectScopedRuleViolationFactory _ruleViolationFactory;
  private readonly IReadOnlyList<string> _declaredNamespaces;
  private readonly string _parentProjectAssemblyName;
  private readonly string _parentProjectRootNamespace;
  private readonly RelativeFilePath _pathRelativeToProjectRoot;
  private readonly ICSharpClass[] _classes;

  public SourceCodeFile(
    IProjectScopedRuleViolationFactory ruleViolationFactory,
    IReadOnlyList<string> declaredNamespaces,
    string parentProjectAssemblyName,
    string parentProjectRootNamespace,
    RelativeFilePath pathRelativeToProjectRoot, 
    ICSharpClass[] classes)
  {
    _ruleViolationFactory = ruleViolationFactory;
    _declaredNamespaces = declaredNamespaces;
    _parentProjectAssemblyName = parentProjectAssemblyName;
    _parentProjectRootNamespace = parentProjectRootNamespace;
    _pathRelativeToProjectRoot = pathRelativeToProjectRoot;
    _classes = classes;
  }

  public void CheckNamespacesCorrectness(IAnalysisReportInProgress report, RuleDescription description)
  {
    //bug get rid of this code here. Move this to rule as another interface
    if (_declaredNamespaces.Count == 0)
    {
      report.Add(_ruleViolationFactory.ProjectScopedRuleViolation(description, ViolationDescription("has no namespace declared"))
      );
    }
    else if (_declaredNamespaces.Count > 1)
    {
      report.Add(_ruleViolationFactory.ProjectScopedRuleViolation(description, ViolationDescription($"declares multiple namespaces: {NamespacesString()}")));
    }
    else if (!_declaredNamespaces.Contains(CorrectNamespace()))
    {
      report.Add(_ruleViolationFactory.ProjectScopedRuleViolation(description, ViolationDescription($"has incorrect namespace {_declaredNamespaces.Single()}"))
      );
    }
  }

  public void CheckMethodsHavingCorrectAttributes(
    IAnalysisReportInProgress report, 
    Pattern classNameInclusionPattern,
    Pattern methodNameInclusionPattern, 
    RuleDescription description)
  {
    foreach (var cSharpClass in _classes)
    {
      if (cSharpClass.NameMatches(classNameInclusionPattern))
      {
        cSharpClass.EvaluateDecorationWithAttributes(report, methodNameInclusionPattern, description);
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