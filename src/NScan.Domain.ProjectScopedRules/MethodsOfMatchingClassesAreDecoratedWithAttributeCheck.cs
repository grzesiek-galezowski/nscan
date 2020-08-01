using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.ProjectScopedRules
{
  public class MethodsOfMatchingClassesAreDecoratedWithAttributeCheck : ISourceCodeFileContentCheck
  {
    private readonly HasAttributesOnRuleComplementDto _ruleDto;

    public MethodsOfMatchingClassesAreDecoratedWithAttributeCheck(HasAttributesOnRuleComplementDto ruleDto)
    {
      _ruleDto = ruleDto;
    }

    public void ApplyTo(ISourceCodeFileInNamespace sourceCodeFile, string ruleDescription, IAnalysisReportInProgress report)
    {
      sourceCodeFile.CheckMethodsHavingCorrectAttributes(
        report, 
        _ruleDto.ClassNameInclusionPattern, 
        _ruleDto.MethodNameInclusionPattern, 
        ruleDescription);
    }
  }
}