using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain.ProjectScopedRules;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.ReadingRules.Ports;
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
      file.Received(1).EvaluateMethodsHavingCorrectAttributes(
        report, 
        dto.ClassNameInclusionPattern, 
        dto.MethodNameInclusionPattern, 
        description);
    }
  }
}