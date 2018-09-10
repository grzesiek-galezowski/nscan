using System;
using System.Collections.Generic;
using FluentAssertions;
using MyTool.App;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.XNSubstitute.Root;
using Xunit;
using static DependencyDescriptions;
using static TddXt.AnyRoot.Root;

namespace MyTool
{
  public class IndependentOfProjectRuleSpecification //todo make it not direct already, why not?
  {
    //todo what about the situation where there is only root?

    [Fact]
    public void ShouldReportOkWhenPathDoesNotContainAnyOfProjectReferencesPointedByIds()
    {
      //GIVEN
      var dependingId = Any.String();
      var dependencyId = Any.String();
      var rule = new IndependentOfProjectRule(dependingId, dependencyId);
      var project1 = ProjectWithIdDifferentThan(dependingId, dependencyId);
      var project2 = ProjectWithIdDifferentThan(dependingId, dependencyId);
      var project3 = ProjectWithIdDifferentThan(dependingId, dependencyId);
      var report = Substitute.For<IAnalysisReportInProgress>();

      var path = new List<IReferencedProject>()
      {
        project1,
        project2,
        project3
      };

      //WHEN
      rule.Check(path, report);

      //THEN
      report.ReceivedNothing();
      //report.Received(1).Ok(IndependentOf(dependingId, dependencyId));
    }

    [Fact]
    public void ShouldReportRuleViolationAndPathWhenDependencyIsDetected()
    {
      //GIVEN
      var dependingId = Any.String();
      var dependencyId = Any.String();
      var rule = new IndependentOfProjectRule(dependingId, dependencyId);
      var project1 = ProjectWithIdDifferentThan(dependingId, dependencyId);
      var project2 = ProjectWithId(dependingId);
      var project3 = ProjectWithIdDifferentThan(dependingId, dependencyId);
      var project4 = ProjectWithId(dependencyId);
      var report = Substitute.For<IAnalysisReportInProgress>();

      var path = new List<IReferencedProject>()
      {
        project1,
        project2,
        project3,
        project4,
      };

      //WHEN
      rule.Check(path, report);

      //THEN
      report.Received(1).ViolationOf(IndependentOf(project2, project4) , ListContaining(project2, project3, project4));
    }

    [Fact]
    public void ShouldNotReportViolationWhenDependencyIsTheOtherWayRound()
    {
      //GIVEN
      var dependingId = Any.String();
      var dependencyId = Any.String();
      var rule = new IndependentOfProjectRule(dependingId, dependencyId);
      var project1 = ProjectWithIdDifferentThan(dependingId, dependencyId);
      var project2 = ProjectWithId(dependencyId);
      var project3 = ProjectWithIdDifferentThan(dependingId, dependencyId);
      var project4 = ProjectWithId(dependingId);
      var report = Substitute.For<IAnalysisReportInProgress>();

      var path = new List<IReferencedProject>()
      {
        project1,
        project2,
        project3,
        project4,
      };

      //WHEN
      rule.Check(path, report);

      //THEN
      report.ReceivedNothing();
    }

    [Fact]
    public void ShouldNotReportViolationWhenDependingIsInPathButNoDependency()
    {
      //GIVEN
      var dependingId = Any.String();
      var dependencyId = Any.String();
      var rule = new IndependentOfProjectRule(dependingId, dependencyId);
      var project1 = ProjectWithIdDifferentThan(dependingId, dependencyId);
      var project2 = ProjectWithId(dependingId);
      var project3 = ProjectWithIdDifferentThan(dependingId, dependencyId);
      var project4 = ProjectWithIdDifferentThan(dependingId, dependencyId);
      var report = Substitute.For<IAnalysisReportInProgress>();

      var path = new List<IReferencedProject>()
      {
        project1,
        project2,
        project3,
        project4,
      };

      //WHEN
      rule.Check(path, report);

      //THEN
      report.ReceivedNothing();
    }

    [Fact]
    public void ShouldNotReportViolationWhenDependencyIsInPathButNoDepending()
    {
      //GIVEN
      var dependingId = Any.String();
      var dependencyId = Any.String();
      var rule = new IndependentOfProjectRule(dependingId, dependencyId);
      var project1 = ProjectWithIdDifferentThan(dependingId, dependencyId);
      var project2 = ProjectWithId(dependencyId);
      var project3 = ProjectWithIdDifferentThan(dependingId, dependencyId);
      var project4 = ProjectWithIdDifferentThan(dependingId, dependencyId);
      var report = Substitute.For<IAnalysisReportInProgress>();

      var path = new List<IReferencedProject>()
      {
        project1,
        project2,
        project3,
        project4,
      };

      //WHEN
      rule.Check(path, report);

      //THEN
      report.ReceivedNothing();
    }


    private static List<IReferencedProject> ListContaining(IReferencedProject project2, IReferencedProject project3, IReferencedProject project4)
    {
      return Arg<List<IReferencedProject>>.That(arg => arg.Should().BeEquivalentTo(new List<IReferencedProject>() {project2, project3, project4}));
    }

    private static IReferencedProject ProjectWithId(String dependingId)
    {
      var referencedProject = Substitute.For<IReferencedProject>();
      referencedProject.HasAssemblyName(dependingId).Returns(true);
      return referencedProject;
    }


    //TODO does not contain dependencyId
    //TODO does not contain dependentId
    //TODO contains both but in wrong order
    //TODO contains both but more than 1 hop away

    private static IReferencedProject ProjectWithIdDifferentThan(params string[] assemblyNames)
    {
      var project1 = Substitute.For<IReferencedProject>();
      foreach (var assemblyName in assemblyNames)
      {
        project1.HasAssemblyName(assemblyName).Returns(false);
      }
      return project1;
    }

  }
}