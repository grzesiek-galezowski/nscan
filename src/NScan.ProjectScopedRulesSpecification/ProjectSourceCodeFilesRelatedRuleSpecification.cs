using System.Collections.Generic;
using FluentAssertions;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScan.ProjectScopedRulesSpecification
{
  public class ProjectSourceCodeFilesRelatedRuleSpecification
  {
    [Fact]
    public void ShouldMakeProjectAnalyzeFilesWithItselfWhenChecked()
    {
      //GIVEN
      string ruleDescription = Any.String();
      var rule = new ProjectSourceCodeFilesRelatedRule(new RuleDescription(ruleDescription), Any.Instance<ISourceCodeFileContentCheck>());
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
      var ruleDescription = Any.String();
      var rule = new ProjectSourceCodeFilesRelatedRule(new RuleDescription(ruleDescription), fileContentCheck);
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
        fileContentCheck.ApplyTo(file1, new RuleDescription(ruleDescription), report);
        fileContentCheck.ApplyTo(file2, new RuleDescription(ruleDescription), report);
        fileContentCheck.ApplyTo(file3, new RuleDescription(ruleDescription), report);
        report.FinishedEvaluatingRule(/* bug investigate */new RuleDescription(ruleDescription));
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
}
