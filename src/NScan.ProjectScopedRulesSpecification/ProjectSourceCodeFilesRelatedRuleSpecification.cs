using System.Collections.Generic;
using FluentAssertions;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NSubstitute;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.ProjectScopedRulesSpecification;

public class ProjectSourceCodeFilesRelatedRuleSpecification
{
  [Fact]
  public void ShouldMakeProjectAnalyzeFilesWithItselfWhenChecked()
  {
    //GIVEN
    var rule = new ProjectSourceCodeFilesRelatedRule(
      Any.Instance<RuleDescription>(), 
      Any.Instance<ISourceCodeFileContentCheck>());
    var report = Any.Instance<IAnalysisReportInProgress>();
    var project = Substitute.For<IProjectScopedRuleTarget>();

    //WHEN
    rule.Check(project, report);

    //THEN
    project.Received(1).AnalyzeFiles(rule, report);
  }

  [Fact]
  public void ShouldAnalyzeNamespaceCorrectnessOnEachAnalyzedFiles()
  {
    //GIVEN
    var fileContentCheck = Substitute.For<ISourceCodeFileContentCheck>();
    var description = Any.Instance<RuleDescription>();
    var rule = new ProjectSourceCodeFilesRelatedRule(description, fileContentCheck);
    var file1 = Substitute.For<ISourceCodeFileInNamespace>();
    var file2 = Substitute.For<ISourceCodeFileInNamespace>();
    var file3 = Substitute.For<ISourceCodeFileInNamespace>();
    var files = new List<ISourceCodeFileInNamespace>
    {
      file1, file2, file3
    };
    var report = Substitute.For<IAnalysisReportInProgress>();

    //WHEN
    rule.Check(files, report);

    //THEN
    Received.InOrder(() =>
    {
      fileContentCheck.ApplyTo(file1, description, report);
      fileContentCheck.ApplyTo(file2, description, report);
      fileContentCheck.ApplyTo(file3, description, report);
      report.FinishedEvaluatingRule(description);
    });
  }

  [Fact]
  public void ShouldAllowGettingItsDescription()
  {
    //GIVEN
    var description = Any.Instance<RuleDescription>();
    var rule = new ProjectSourceCodeFilesRelatedRule(
      description, 
      Any.Instance<ISourceCodeFileContentCheck>());

    //WHEN
    var ruleAsString = rule.Description();

    //THEN
    ruleAsString.Should().Be(description);
  }
}