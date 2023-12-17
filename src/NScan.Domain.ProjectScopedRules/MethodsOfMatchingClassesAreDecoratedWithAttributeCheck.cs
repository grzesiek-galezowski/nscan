using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.ProjectScopedRules;

public class MethodsOfMatchingClassesAreDecoratedWithAttributeCheck(HasAttributesOnRuleComplementDto ruleDto)
  : ISourceCodeFileContentCheck
{
  public void ApplyTo(
    ISourceCodeFileInNamespace sourceCodeFile,
    RuleDescription description,
    IAnalysisReportInProgress report)
  {
    sourceCodeFile.CheckMethodsHavingCorrectAttributes(
      report, 
      ruleDto.ClassNameInclusionPattern, 
      ruleDto.MethodNameInclusionPattern, 
      description);
  }
}
