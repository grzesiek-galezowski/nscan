using System;
using System.Collections.Generic;
using MyTool.App;
using NSubstitute;
using NSubstitute.Core;
using TddXt.XNSubstitute.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace MyTool
{
  public class DirectIndependentOfProjectRuleSpecification //todo make it not direct already, why not?
  {
    //todo what about the situation where there is only root?

    [Fact]
    public void ShouldDoNothingWhenPathDoesNotContainAnyOfProjectReferencesPointedByIds()
    {
      //GIVEN
      var dependingId = Any.ProjectId();
      var dependencyId = Any.ProjectId();
      var rule = new DirectIndependentOfProjectRule(dependingId, dependencyId);
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
    public void ShouldReportPathWhen()
    {
      //GIVEN
      var dependingId = Any.ProjectId();
      var dependencyId = Any.ProjectId();
      var rule = new DirectIndependentOfProjectRule(dependingId, dependencyId);
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
      report.Received(1).ViolationOf(
        "[" + dependingId + "] independentOf [" + dependencyId + "]", 
        new List<IReferencedProject>() {project2, project3, project4});
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