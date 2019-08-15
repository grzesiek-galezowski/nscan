﻿using System.Collections.Generic;
using FluentAssertions;
using NScan.Domain.Domain.ProjectScopedRules;
using NScan.Domain.Domain.Root;
using NScan.SharedKernel.SharedKernel;
using NScanSpecification.Lib;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.ProjectScopedRules
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
      var projectAssemblyNamePattern = Any.Pattern();
      var fileContentCheck = Substitute.For<ISourceCodeFileContentCheck>();
      var ruleDescription = Any.String();
      var rule = new ProjectSourceCodeFilesRelatedRule(ruleDescription, fileContentCheck);
      var file1 = Substitute.For<ISourceCodeFile>();
      var file2 = Substitute.For<ISourceCodeFile>();
      var file3 = Substitute.For<ISourceCodeFile>();
      var files = new List<ISourceCodeFile>
      {
        file1, file2, file3
      };
      var report = Any.Instance<IAnalysisReportInProgress>();

      //WHEN
      rule.Check(files, report);

      //THEN
      Received.InOrder(() =>
      {
        fileContentCheck.ApplyTo(file1, ruleDescription, report);
        fileContentCheck.ApplyTo(file2, ruleDescription, report);
        fileContentCheck.ApplyTo(file3, ruleDescription, report);
        report.FinishedChecking(projectAssemblyNamePattern + " hasCorrectNamespaces");
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
