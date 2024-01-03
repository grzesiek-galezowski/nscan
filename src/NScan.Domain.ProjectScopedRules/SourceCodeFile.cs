using System.IO;
using AtmaFileSystem;
using LanguageExt;
using NScan.Lib;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRules;

public class SourceCodeFile(
  IProjectScopedRuleViolationFactory ruleViolationFactory,
  Seq<string> declaredNamespaces,
  string parentProjectAssemblyName,
  string parentProjectRootNamespace,
  RelativeFilePath pathRelativeToProjectRoot,
  Seq<ICSharpClass> classes)
  : ISourceCodeFileInNamespace
{
  public void CheckNamespacesCorrectness(IAnalysisReportInProgress report, RuleDescription description)
  {
    if (declaredNamespaces.Count == 0)
    {
      report.Add(ruleViolationFactory.ProjectScopedRuleViolation(
        description, ViolationDescription("has no namespace declared")));
    }
    else if (declaredNamespaces.Count > 1)
    {
      report.Add(ruleViolationFactory.ProjectScopedRuleViolation(description, ViolationDescription($"declares multiple namespaces: {NamespacesString()}")));
    }
    else if (!declaredNamespaces.Contains(CorrectNamespace()))
    {
      report.Add(ruleViolationFactory.ProjectScopedRuleViolation(description, ViolationDescription($"has incorrect namespace {declaredNamespaces.Single()}"))
      );
    }
  }

  public void CheckMethodsHavingCorrectAttributes(
    IAnalysisReportInProgress report, 
    Pattern classNameInclusionPattern,
    Pattern methodNameInclusionPattern, 
    RuleDescription description)
  {
    foreach (var cSharpClass in classes)
    {
      if (cSharpClass.NameMatches(classNameInclusionPattern))
      {
        cSharpClass.EvaluateDecorationWithAttributes(report, methodNameInclusionPattern, description);
      }
    }
  }

  private string NamespacesString()
  {
    return string.Join(", ", declaredNamespaces);
  }

  private string ViolationDescription(string reason)
  {
    return parentProjectAssemblyName + " has root namespace " +
           parentProjectRootNamespace + " but the file "
           + pathRelativeToProjectRoot + " " + reason;
  }

  private string CorrectNamespace()
  {
    if (!pathRelativeToProjectRoot.ParentDirectory().HasValue)
    {
      return parentProjectRootNamespace;
    }
    else
    {
      var fileLocationRelativeToProjectDir = pathRelativeToProjectRoot.ParentDirectory().Value();
      return
        $"{parentProjectRootNamespace}.{fileLocationRelativeToProjectDir.ToString().Replace(Path.DirectorySeparatorChar, '.')}";
    }
  }

  public override string ToString()
  {
    return pathRelativeToProjectRoot.ToString();
  }
}
