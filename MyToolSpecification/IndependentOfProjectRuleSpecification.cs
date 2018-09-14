using System.Collections.Generic;
using FluentAssertions;
using MyTool.App;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.XNSubstitute.Root;
using Xunit;
using static MyTool.DependencyDescriptions;
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
      var dependingPattern = Any.String();
      var dependencyPattern = Any.String();
      var rule = new IndependentOfProjectRule(dependingPattern, dependencyPattern);
      var project1 = ProjectWithNameNotMatching(dependingPattern, dependencyPattern);
      var project2 = ProjectWithNameNotMatching(dependingPattern, dependencyPattern);
      var project3 = ProjectWithNameNotMatching(dependingPattern, dependencyPattern);
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
      XReceived.Only(() => report.Ok(IndependentOf(dependingPattern, dependencyPattern)));
    }

    [Fact]
    public void ShouldReportRuleViolationAndPathWhenDependencyIsDetected()
    {
      //GIVEN
      var dependingPattern = Any.String();
      var dependencyPattern = Any.String();
      var rule = new IndependentOfProjectRule(dependingPattern, dependencyPattern);
      var project1 = ProjectWithNameNotMatching(dependingPattern, dependencyPattern);
      var project2 = ProjectWithNameMatching(dependingPattern);
      var project3 = ProjectWithNameNotMatching(dependingPattern, dependencyPattern);
      var project4 = ProjectWithNameMatching(dependencyPattern);
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
        report.ViolationOf(
          IndependentOf(dependingPattern, dependencyPattern), 
          ListContaining(project2, project3, project4))
      );
    }

    [Fact]
    public void ShouldReportOkWhenDependencyIsTheOtherWayRound()
    {
      //GIVEN
      var dependingPattern = Any.String();
      var dependencyPattern = Any.String();
      var rule = new IndependentOfProjectRule(dependingPattern, dependencyPattern);
      var project1 = ProjectWithNameNotMatching(dependingPattern, dependencyPattern);
      var project2 = ProjectWithNameMatching(dependencyPattern);
      var project3 = ProjectWithNameNotMatching(dependingPattern, dependencyPattern);
      var project4 = ProjectWithNameMatching(dependingPattern);
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
      XReceived.Only(() => report.Ok(IndependentOf(dependingPattern, dependencyPattern)));
    }

    [Fact]
    public void ShouldReportOkWhenDependingIsInPathButNoDependency()
    {
      //GIVEN
      var dependingPattern = Any.String();
      var dependencyPattern = Any.String();
      var rule = new IndependentOfProjectRule(dependingPattern, dependencyPattern);
      var project1 = ProjectWithNameNotMatching(dependingPattern, dependencyPattern);
      var project2 = ProjectWithNameMatching(dependingPattern);
      var project3 = ProjectWithNameNotMatching(dependingPattern, dependencyPattern);
      var project4 = ProjectWithNameNotMatching(dependingPattern, dependencyPattern);
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
      XReceived.Only(() => report.Ok(IndependentOf(dependingPattern, dependencyPattern)));
    }

    [Fact]
    public void ShouldReportOkWhenDependencyIsInPathButNoDepending()
    {
      //GIVEN
      var dependingPattern = Any.String();
      var dependencyPattern = Any.String();
      var rule = new IndependentOfProjectRule(dependingPattern, dependencyPattern);
      var project1 = ProjectWithNameNotMatching(dependingPattern, dependencyPattern);
      var project2 = ProjectWithNameMatching(dependencyPattern);
      var project3 = ProjectWithNameNotMatching(dependingPattern, dependencyPattern);
      var project4 = ProjectWithNameNotMatching(dependingPattern, dependencyPattern);
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
      XReceived.Only(() => report.Ok(IndependentOf(dependingPattern, dependencyPattern)));
    }

    [Fact]
    public void ShouldReportViolationWhenDependingAndDependencyMatchTheSamePattern()
    {
      //GIVEN
      var dependingPattern = Any.String();
      var dependencyPattern = dependingPattern;
      var rule = new IndependentOfProjectRule(dependingPattern, dependencyPattern);
      var project1 = ProjectWithNameNotMatching(dependingPattern, dependencyPattern);
      var project2 = ProjectWithNameMatching(dependingPattern);
      var project3 = ProjectWithNameNotMatching(dependingPattern, dependencyPattern);
      var project4 = ProjectWithNameMatching(dependencyPattern);
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
        report.ViolationOf(
          IndependentOf(dependingPattern, dependencyPattern),
          ListContaining(project2, project3, project4))
      );
    }

    //bug Same pattern twice should be OK, since one can be XXX.Y1 and second can be XXX.Y2

    private static List<IReferencedProject> ListContaining(IReferencedProject project2, IReferencedProject project3, IReferencedProject project4)
    {
      return Arg<List<IReferencedProject>>.That(arg => arg.Should().BeEquivalentTo(new List<IReferencedProject>() {project2, project3, project4}));
    }

    private static IReferencedProject ProjectWithNameMatching(string dependingName)
    {
      var referencedProject = Substitute.For<IReferencedProject>();
      referencedProject.HasAssemblyNameMatching(dependingName).Returns(true);
      return referencedProject;
    }


    //TODO does not contain dependencyId
    //TODO does not contain dependentId
    //TODO contains both but in wrong order
    //TODO contains both but more than 1 hop away

    private static IReferencedProject ProjectWithNameNotMatching(params string[] assemblyNames)
    {
      var project1 = Substitute.For<IReferencedProject>();
      foreach (var assemblyName in assemblyNames)
      {
        project1.HasAssemblyNameMatching(assemblyName).Returns(false);
      }
      return project1;
    }

  }
}