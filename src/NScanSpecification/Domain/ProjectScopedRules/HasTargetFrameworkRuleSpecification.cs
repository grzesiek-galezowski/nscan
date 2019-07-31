using System;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.NScan.Domain.ProjectScopedRules;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.XNSubstitute.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.ProjectScopedRules
{
  public class HasTargetFrameworkRuleSpecification
  {
    [Fact]
    public void ShouldReportNothingWhenProjectDoesNotMatchAssemblyName()
    {
      //GIVEN
      var projectAssemblyNamePattern = Any.Pattern();
      var rule = new HasTargetFrameworkRule(projectAssemblyNamePattern, Any.String());
      var project = Substitute.For<IProjectScopedRuleTarget>();
      var analysisReportInProgress = Substitute.For<IAnalysisReportInProgress>();

      project.HasProjectAssemblyNameMatching(projectAssemblyNamePattern).Returns(false);

      //WHEN
      rule.Check(project, analysisReportInProgress);

      //THEN
      analysisReportInProgress.ReceivedNothing();
    }
    
    [Fact]
    public void ShouldReportNothingWhenProjectMatchesAssemblyNameAndTargetFramework()
    {
      //GIVEN
      var projectAssemblyNamePattern = Any.Pattern();
      var targetFramework = Any.String();
      var rule = new HasTargetFrameworkRule(projectAssemblyNamePattern, targetFramework);
      var project = Substitute.For<IProjectScopedRuleTarget>();
      var analysisReportInProgress = Substitute.For<IAnalysisReportInProgress>();

      project.HasProjectAssemblyNameMatching(projectAssemblyNamePattern).Returns(true);

      //WHEN
      rule.Check(project, analysisReportInProgress);

      //THEN
      project.Received(1).ValidateTargetFrameworkWith((ITargetFrameworkCheck)rule, analysisReportInProgress);
    }
  }

}