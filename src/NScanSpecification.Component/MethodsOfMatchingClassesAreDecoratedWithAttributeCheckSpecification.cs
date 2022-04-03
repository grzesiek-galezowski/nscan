using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScanSpecification.Component;

public class MethodsOfMatchingClassesAreDecoratedWithAttributeCheckSpecification
{
  [Fact]
  public void ShouldEvaluateFileMethodsForBeingDecoratedWithAttributesWhenAppliedToThisFile()
  {
    //GIVEN
    var dto = Any.Instance<HasAttributesOnRuleComplementDto>();
    var check = new MethodsOfMatchingClassesAreDecoratedWithAttributeCheck(dto);
    var file = Substitute.For<ISourceCodeFileInNamespace>();
    var ruleDescription = Any.Instance<RuleDescription>();
    var report = Any.Instance<IAnalysisReportInProgress>();
      
    //WHEN
    check.ApplyTo(file, ruleDescription, report);

    //THEN
    file.Received(1).CheckMethodsHavingCorrectAttributes(
      report, 
      dto.ClassNameInclusionPattern, 
      dto.MethodNameInclusionPattern, ruleDescription);
  }
}
