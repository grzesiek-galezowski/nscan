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
      var dependingName = Any.String();
      var dependencyName = Any.String();
      var rule = new IndependentOfProjectRule(dependingName, dependencyName);
      var project1 = ProjectWithIdDifferentThan(dependingName, dependencyName);
      var project2 = ProjectWithIdDifferentThan(dependingName, dependencyName);
      var project3 = ProjectWithIdDifferentThan(dependingName, dependencyName);
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
      XReceived.Only(() => report.Ok(IndependentOf(dependingName, dependencyName)));
    }

    [Fact]
    public void ShouldReportRuleViolationAndPathWhenDependencyIsDetected()
    {
      //GIVEN
      var dependingName = Any.String();
      var dependencyName = Any.String();
      var rule = new IndependentOfProjectRule(dependingName, dependencyName);
      var project1 = ProjectWithIdDifferentThan(dependingName, dependencyName);
      var project2 = ProjectWithId(dependingName);
      var project3 = ProjectWithIdDifferentThan(dependingName, dependencyName);
      var project4 = ProjectWithId(dependencyName);
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
      XReceived.Only(() => 
        report.Received(1).ViolationOf(
          IndependentOf(dependingName, dependencyName), 
          ListContaining(project2, project3, project4))
      );
    }

    [Fact]
    public void ShouldReportOkWhenDependencyIsTheOtherWayRound()
    {
      //GIVEN
      var dependingName = Any.String();
      var dependencyName = Any.String();
      var rule = new IndependentOfProjectRule(dependingName, dependencyName);
      var project1 = ProjectWithIdDifferentThan(dependingName, dependencyName);
      var project2 = ProjectWithId(dependencyName);
      var project3 = ProjectWithIdDifferentThan(dependingName, dependencyName);
      var project4 = ProjectWithId(dependingName);
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
      XReceived.Only(() => report.Ok(IndependentOf(dependingName, dependencyName)));
    }

    [Fact]
    public void ShouldReportOkWhenDependingIsInPathButNoDependency()
    {
      //GIVEN
      var dependingName = Any.String();
      var dependencyName = Any.String();
      var rule = new IndependentOfProjectRule(dependingName, dependencyName);
      var project1 = ProjectWithIdDifferentThan(dependingName, dependencyName);
      var project2 = ProjectWithId(dependingName);
      var project3 = ProjectWithIdDifferentThan(dependingName, dependencyName);
      var project4 = ProjectWithIdDifferentThan(dependingName, dependencyName);
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
      XReceived.Only(() => report.Ok(IndependentOf(dependingName, dependencyName)));
    }

    [Fact]
    public void ShouldReportOkWhenDependencyIsInPathButNoDepending()
    {
      //GIVEN
      var dependingName = Any.String();
      var dependencyName = Any.String();
      var rule = new IndependentOfProjectRule(dependingName, dependencyName);
      var project1 = ProjectWithIdDifferentThan(dependingName, dependencyName);
      var project2 = ProjectWithId(dependencyName);
      var project3 = ProjectWithIdDifferentThan(dependingName, dependencyName);
      var project4 = ProjectWithIdDifferentThan(dependingName, dependencyName);
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
      XReceived.Only(() => report.Ok(IndependentOf(dependingName, dependencyName)));
    }


    private static List<IReferencedProject> ListContaining(IReferencedProject project2, IReferencedProject project3, IReferencedProject project4)
    {
      return Arg<List<IReferencedProject>>.That(arg => arg.Should().BeEquivalentTo(new List<IReferencedProject>() {project2, project3, project4}));
    }

    private static IReferencedProject ProjectWithId(string dependingName)
    {
      var referencedProject = Substitute.For<IReferencedProject>();
      referencedProject.HasAssemblyName(dependingName).Returns(true);
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