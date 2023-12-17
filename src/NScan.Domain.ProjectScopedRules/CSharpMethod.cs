using System.Linq;
using NScan.Lib;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingCSharpSourceCode;

namespace NScan.ProjectScopedRules;

public class CSharpMethod(
  MethodDeclarationInfo methodDeclarationInfo,
  IProjectScopedRuleViolationFactory violationFactory)
  : ICSharpMethod
{
  public bool NameMatches(Pattern methodNameInclusionPattern)
  {
    return methodNameInclusionPattern.IsMatchedBy(methodDeclarationInfo.Name);
  }

  public void EvaluateMethodsHavingCorrectAttributes(IAnalysisReportInProgress report, string parentClassName, RuleDescription description)
  {
    if (!methodDeclarationInfo.Attributes.Any())
    {
      report.Add(violationFactory.ProjectScopedRuleViolation(description, $"Method {methodDeclarationInfo.Name} in class {parentClassName} does not have any attribute"));
    }
  }
}
