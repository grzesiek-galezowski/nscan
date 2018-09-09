using System.Collections.Generic;
using FluentAssertions;
using MyTool.App;
using NSubstitute;
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
    public void ShouldDoNothingWhenPathDoesNotContainAnyOfProjectReferencesPointedByIds()
    {
      //GIVEN
      var dependingId = Any.ProjectId();
      var dependencyId = Any.ProjectId();
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
    }

    [Fact]
    public void ShouldReportRuleViolationAndPathWhenDependencyIsDetected()
    {
      //GIVEN
      var dependingId = Any.ProjectId();
      var dependencyId = Any.ProjectId();
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
      var dependingId = Any.ProjectId();
      var dependencyId = Any.ProjectId();
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
      var dependingId = Any.ProjectId();
      var dependencyId = Any.ProjectId();
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
      var dependingId = Any.ProjectId();
      var dependencyId = Any.ProjectId();
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

    private static IReferencedProject ProjectWithId(ProjectId dependingId)
    {
      var referencedProject = Substitute.For<IReferencedProject>();
      referencedProject.Has(dependingId).Returns(true);
      return referencedProject;
    }


    //TODO does not contain dependencyId
    //TODO does not contain dependentId
    //TODO contains both but in wrong order
    //TODO contains both but more than 1 hop away

    private static IReferencedProject ProjectWithIdDifferentThan(params ProjectId[] ids)
    {
      var project1 = Substitute.For<IReferencedProject>();
      foreach (var projectId in ids)
      {
        project1.Has(projectId).Returns(false);
      }
      return project1;
    }

  }
}