using System;
using System.Linq;
using NScan.Adapter.ReadingCSharpSolution.ReadingCSharpSourceCode;
using NScan.Lib;
using NScan.SharedKernel.SharedKernel;

namespace TddXt.NScan.Domain.Root
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
      return namePattern.IsMatch(_classDeclarationInfo.Name);
    }

    public void EvaluateDecorationWithAttributes(
      IAnalysisReportInProgress report, 
      Pattern methodNameInclusionPattern,
      string ruleDescription)
    {
      foreach (var method in _methods.Where(m => m.NameMatches(methodNameInclusionPattern)))
      {
        method.EvaluateMethodsHavingCorrectAttributes(report, _classDeclarationInfo.Name, ruleDescription);
      }
    }
  }
}