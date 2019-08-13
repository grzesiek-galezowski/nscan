using NScan.SharedKernel.SharedKernel;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain.ProjectScopedRules;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.ProjectScopedRules
{
  public class CorrectNamespacesInFileCheckSpecification
  {
    [Fact]
    public void ShouldEvaluateNamespacesCorrectnessOnAFileWhenAppliedToThatFile()
    {
      //GIVEN
      var check = new CorrectNamespacesInFileCheck();
      var file = Substitute.For<ISourceCodeFileInNamespace>();
      var description = Any.String();
      var report = Any.Instance<IAnalysisReportInProgress>();
      
      //WHEN
      check.ApplyTo(file, description, report);

      //THEN
      file.Received(1).EvaluateNamespacesCorrectness(report, description);
    }
  }
}