using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScanSpecification.Component
{
  public class MethodsOfMatchingClassesAreDecoratedWithAttributeCheckSpecification
  {
    [Fact]
    public void ShouldEvaluateFileMethodsForBeingDecoratedWithAttributesWhenAppliedToThisFile()
    {
      //GIVEN
      var dto = Any.Instance<HasAttributesOnRuleComplementDto>();
      var check = new MethodsOfMatchingClassesAreDecoratedWithAttributeCheck(dto);
      var file = Substitute.For<ISourceCodeFileInNamespace>();
      var description = Any.String();
      var report = Any.Instance<IAnalysisReportInProgress>();
      
      //WHEN
      check.ApplyTo(file, description, report);

      //THEN
      file.Received(1).CheckMethodsHavingCorrectAttributes(
        report, 
        dto.ClassNameInclusionPattern, 
        dto.MethodNameInclusionPattern, 
        description);
    }
  }
}