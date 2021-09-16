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
      var rule = new ProjectSourceCodeFilesRelatedRule(Any.String(), Any.Instance<ISourceCodeFileContentCheck>());
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
      var rule = new ProjectSourceCodeFilesRelatedRule(ruleDescription, fileContentCheck);
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
        fileContentCheck.ApplyTo(file1, ruleDescription, report);
        fileContentCheck.ApplyTo(file2, ruleDescription, report);
        fileContentCheck.ApplyTo(file3, ruleDescription, report);
        report.FinishedEvaluatingRule(/* bug investigate */new RuleDescription(ruleDescription));
      });
    }

    [Fact]
    public void ShouldReturnRuleDescriptionWhenConvertedToString()
    {
      //GIVEN
      var ruleDescription = Any.String();
      var rule = new ProjectSourceCodeFilesRelatedRule(ruleDescription, Any.Instance<ISourceCodeFileContentCheck>());

      //WHEN
      var ruleAsString = rule.ToString();

      //THEN
      ruleAsString.Should().Be(ruleDescription);
    }
  }
}
