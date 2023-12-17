using System.Linq;
using NScan.Lib;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingCSharpSourceCode;

namespace NScan.ProjectScopedRules;

public class CSharpClass(ClassDeclarationInfo classDeclarationInfo, ICSharpMethod[] methods)
  : ICSharpClass
{
  public bool NameMatches(Pattern namePattern)
  {
    return namePattern.IsMatchedBy(classDeclarationInfo.Name);
  }

  public void EvaluateDecorationWithAttributes(
    IAnalysisReportInProgress report, 
    Pattern methodNameInclusionPattern, RuleDescription description)
  {
    foreach (var method in methods.Where(m => m.NameMatches(methodNameInclusionPattern)))
    {
      method.EvaluateMethodsHavingCorrectAttributes(report, classDeclarationInfo.Name, description);
    }
  }
}
