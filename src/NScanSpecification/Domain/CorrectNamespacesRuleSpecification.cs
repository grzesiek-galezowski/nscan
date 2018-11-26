using System;
using System.Collections.Generic;
using System.Data;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain;
using TddXt.NScan.RuleInputData;
using TddXt.XNSubstitute.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
{
  public class CorrectNamespacesRuleSpecification
  {
    [Fact]
    public void ShouldMakeAnalyzeFilesWithItselfWhenProjectMatchesAPattern()
    {
      //GIVEN
      var dto = Any.Instance<CorrectNamespacesRuleComplementDto>();
      var rule = new CorrectNamespacesRule(dto);
      var report = Any.Instance<IAnalysisReportInProgress>();
      var project = Substitute.For<IProjectScopedRuleTarget>();

      project.HasProjectAssemblyNameMatching(dto.ProjectAssemblyNamePattern).Returns(true);

      //WHEN
      rule.Check(project, report);

      //THEN
      project.Received(1).AnalyzeFiles(rule, report);
    }

    [Fact]
    public void ShouldNotMakeAnalyzeFilesWithItselfWhenProjectDoesNotMatchAPattern()
    {
      //GIVEN
      var dto = Any.Instance<CorrectNamespacesRuleComplementDto>();
      var rule = new CorrectNamespacesRule(dto);
      var report = Any.Instance<IAnalysisReportInProgress>();
      var project = Substitute.For<IProjectScopedRuleTarget>();

      project.HasProjectAssemblyNameMatching(dto.ProjectAssemblyNamePattern).Returns(false);

      //WHEN
      rule.Check(project, report);

      //THEN
      project.DidNotReceive().AnalyzeFiles(Arg.Any<IProjectScopedRule>(), Arg.Any<IAnalysisReportInProgress>());
    }
  }
}
