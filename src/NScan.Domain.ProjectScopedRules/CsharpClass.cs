using System.Linq;
using NScan.Lib;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingCSharpSourceCode;

namespace NScan.ProjectScopedRules
{
  public class CSharpClass : ICSharpClass
  {
    private readonly ClassDeclarationInfo _classDeclarationInfo;
    private readonly ICSharpMethod[] _methods;

    public CSharpClass(ClassDeclarationInfo classDeclarationInfo, ICSharpMethod[] methods)
    {
      _classDeclarationInfo = classDeclarationInfo;
      _methods = methods;
    }

    public bool NameMatches(Pattern namePattern)
    {
      return namePattern.IsMatchedBy(_classDeclarationInfo.Name);
    }

    public void EvaluateDecorationWithAttributes(
      IAnalysisReportInProgress report, 
      Pattern methodNameInclusionPattern, RuleDescription description)
    {
      foreach (var method in _methods.Where(m => m.NameMatches(methodNameInclusionPattern)))
      {
        method.EvaluateMethodsHavingCorrectAttributes(report, _classDeclarationInfo.Name, description);
      }
    }
  }
}
