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
  public class DirectIndependentOfProjectRuleSpecification
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