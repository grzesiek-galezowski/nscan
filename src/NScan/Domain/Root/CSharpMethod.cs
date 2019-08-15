using System.Linq;
using NScan.Adapter.ReadingCSharpSolution.ReadingCSharpSourceCode;
using NScan.Lib;
using NScan.SharedKernel.SharedKernel;
using TddXt.NScan.Domain.ProjectScopedRules;

namespace TddXt.NScan.Domain.Root
{
  public class CSharpMethod : ICSharpMethod
  {
    private readonly MethodDeclarationInfo _methodDeclarationInfo;
    private readonly IProjectScopedRuleViolationFactory _violationFactory;

    public CSharpMethod(MethodDeclarationInfo methodDeclarationInfo,
      IProjectScopedRuleViolationFactory violationFactory)
    {
      _methodDeclarationInfo = methodDeclarationInfo;
      _violationFactory = violationFactory;
    }

    public bool NameMatches(Pattern methodNameInclusionPattern)
    {
      return methodNameInclusionPattern.IsMatch(_methodDeclarationInfo.Name);
    }

    public void EvaluateMethodsHavingCorrectAttributes(IAnalysisReportInProgress report, string parentClassName,
      string ruleDescription)
    {
      if (!_methodDeclarationInfo.Attributes.Any())
      {
        report.Add(_violationFactory.ProjectScopedRuleViolation(ruleDescription, $"Method {_methodDeclarationInfo.Name} in class {parentClassName} does not have any attribute"));
      }
    }
  }
}