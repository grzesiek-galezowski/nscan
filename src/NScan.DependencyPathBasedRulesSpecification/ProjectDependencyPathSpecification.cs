using NScan.DependencyPathBasedRules;
using NScanSpecification.Lib;

namespace NScan.DependencyPathBasedRulesSpecification;

public class ProjectDependencyPathSpecification
{
  [Fact]
  public void ShouldReturnSegmentBetweenTwoResultsAsSegmentStartingWithDependingAndEndingWithDependency()
  {
    //GIVEN
    var projects = Any.ReadOnlyList<IDependencyPathBasedRuleTarget>().ToSeq();
    var projectsSegment = Any.OtherThan(projects);
    var path = new ProjectDependencyPath(projects, Any.Instance<IProjectFoundSearchResultFactory>());
    var depending = Substitute.For<IProjectSearchResult>();
    var dependency = Any.Instance<IProjectSearchResult>();

    depending.SegmentEndingWith(dependency, projects).Returns(projectsSegment);

    //WHEN
    var actualSegment = path.SegmentBetween(depending, dependency);
      
    //THEN
    actualSegment.Should().BeEquivalentTo(projectsSegment);
  }
    
  [Fact]
  public void ShouldReturnResultWithFoundProjectAndItsIndexWhenTheProjectMatchesCondition()
  {
    //GIVEN
    var project1 = Any.Instance<IDependencyPathBasedRuleTarget>();
    var project2 = Any.Instance<IDependencyPathBasedRuleTarget>();
    var project3 = Any.Instance<IDependencyPathBasedRuleTarget>();
    var projects = new List<IDependencyPathBasedRuleTarget>
    {
      project1, project2, project3
    }.ToSeq();
    var searchResultFactory = Substitute.For<IProjectFoundSearchResultFactory>();
    var depending = Substitute.For<IProjectSearchResult>();
    var condition = Substitute.For<IDescribedDependencyCondition>();
    var foundResult = Any.Instance<IProjectSearchResult>();

    var path = new ProjectDependencyPath(projects, searchResultFactory);

    condition.Matches(depending, project1).Returns(false);
    condition.Matches(depending, project2).Returns(true);
    condition.Matches(depending, project3).Returns(false);
    searchResultFactory.ItemFound(project2, 1).Returns(foundResult);

    //WHEN
    var actualResult = path.AssemblyMatching(condition, depending);
      
    //THEN
    actualResult.Should().Be(foundResult);
  }
    
  [Fact]
  public void ShouldReturnNotFoundResultWhenNoneOfTheProjectsMatchCondition()
  {
    //GIVEN
    var project1 = Any.Instance<IDependencyPathBasedRuleTarget>();
    var project2 = Any.Instance<IDependencyPathBasedRuleTarget>();
    var project3 = Any.Instance<IDependencyPathBasedRuleTarget>();
    var projects = new List<IDependencyPathBasedRuleTarget>
    {
      project1, project2, project3
    }.ToSeq();
    var searchResultFactory = Substitute.For<IProjectFoundSearchResultFactory>();
    var depending = Substitute.For<IProjectSearchResult>();
    var condition = Substitute.For<IDescribedDependencyCondition>();
    var notFoundResult = Any.Instance<IProjectSearchResult>();

    var path = new ProjectDependencyPath(projects, searchResultFactory);

    condition.Matches(depending, project1).Returns(false);
    condition.Matches(depending, project2).Returns(false);
    condition.Matches(depending, project3).Returns(false);
    searchResultFactory.ItemNotFound().Returns(notFoundResult);

    //WHEN
    var actualResult = path.AssemblyMatching(condition, depending);
      
    //THEN
    actualResult.Should().Be(notFoundResult);
  }

  [Fact]
  public void ShouldReturnResultWithFoundProjectAndItsIndexWhenTheProjectMatchesNamePattern()
  {
    //GIVEN
    var pattern = Any.Pattern();
    var project1 = Substitute.For<IDependencyPathBasedRuleTarget>();
    var project2 = Substitute.For<IDependencyPathBasedRuleTarget>();
    var project3 = Substitute.For<IDependencyPathBasedRuleTarget>();
    var projects = new List<IDependencyPathBasedRuleTarget>
    {
      project1, project2, project3
    }.ToSeq();
    var searchResultFactory = Substitute.For<IProjectFoundSearchResultFactory>();
    var foundResult = Any.Instance<IProjectSearchResult>();

    var path = new ProjectDependencyPath(projects, searchResultFactory);

    project1.HasProjectAssemblyNameMatching(pattern).Returns(false);
    project2.HasProjectAssemblyNameMatching(pattern).Returns(true);
    project3.HasProjectAssemblyNameMatching(pattern).Returns(false);
    searchResultFactory.ItemFound(project2, 1).Returns(foundResult);

    //WHEN
    var actualResult = path.AssemblyWithNameMatching(pattern);

    //THEN
    actualResult.Should().Be(foundResult);
  }

  [Fact]
  public void ShouldReturnNotFoundResultWhenNoneOfTheProjectsMatchNamePattern()
  {
    //GIVEN
    var pattern = Any.Pattern();
    var project1 = Substitute.For<IDependencyPathBasedRuleTarget>();
    var project2 = Substitute.For<IDependencyPathBasedRuleTarget>();
    var project3 = Substitute.For<IDependencyPathBasedRuleTarget>();
    var projects = new List<IDependencyPathBasedRuleTarget>
    {
      project1, project2, project3
    }.ToSeq();
    var searchResultFactory = Substitute.For<IProjectFoundSearchResultFactory>();
    var notFoundResult = Any.Instance<IProjectSearchResult>();

    var path = new ProjectDependencyPath(projects, searchResultFactory);

    project1.HasProjectAssemblyNameMatching(pattern).Returns(false);
    project2.HasProjectAssemblyNameMatching(pattern).Returns(false);
    project3.HasProjectAssemblyNameMatching(pattern).Returns(false);
    searchResultFactory.ItemNotFound().Returns(notFoundResult);

    //WHEN
    var actualResult = path.AssemblyWithNameMatching(pattern);

    //THEN
    actualResult.Should().Be(notFoundResult);
  }

}
