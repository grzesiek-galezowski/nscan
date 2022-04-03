using NScan.ProjectScopedRules;
using NScan.SharedKernel;

namespace NScan.ProjectScopedRulesSpecification;

public class CorrectNamespacesInFileCheckSpecification
{
  [Fact]
  public void ShouldEvaluateNamespacesCorrectnessOnAFileWhenAppliedToThatFile()
  {
    //GIVEN
    var check = new CorrectNamespacesInFileCheck();
    var file = Substitute.For<ISourceCodeFileInNamespace>();
    var ruleDescription = Any.Instance<RuleDescription>();
    var report = Any.Instance<IAnalysisReportInProgress>();
      
    //WHEN
    check.ApplyTo(file, ruleDescription, report);

    //THEN
    file.Received(1).CheckNamespacesCorrectness(report, ruleDescription);
  }
}
