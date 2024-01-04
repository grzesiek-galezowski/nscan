using LanguageExt;
using NScan.DependencyPathBasedRules;
using NScanSpecification.Lib;

namespace NScan.DependencyPathBasedRulesSpecification;

public class ProjectFoundSearchResultSpecification
{
  [Fact]
  public void ShouldExistWhenWrappingNonNullInstance()
  {
    //GIVEN
    var result = new ProjectFoundSearchResult(Any.Instance<IDependencyPathBasedRuleTarget>(), Any.Integer());

    //WHEN
    var exists = result.Exists();

    //THEN
    exists.Should().BeTrue();
  }

  [Fact]
  public void ShouldExistAfterAnotherResultWhenItExistsAndAnotherResultIsBeforeItsccurenceIndex()
  {
    //GIVEN
    var resultOccurenceIndex = Any.Integer();
    var result = new ProjectFoundSearchResult(Any.Instance<IDependencyPathBasedRuleTarget>(), resultOccurenceIndex);
    var anotherResult = Substitute.For<IProjectSearchResult>();

    anotherResult.IsNotAfter(resultOccurenceIndex).Returns(true);

    //WHEN
    var exists = result.IsNotBefore(anotherResult);

    //THEN
    exists.Should().BeTrue();
  }

  [Fact]
  public void ShouldReturnSegmentEndingWithAnotherResultAsTerminatedSegmentStartingFromItsIndex()
  {
    //GIVEN
    var resultOccurenceIndex = Any.Integer();
    var result = new ProjectFoundSearchResult(Any.Instance<IDependencyPathBasedRuleTarget>(), resultOccurenceIndex);
    var anotherResult = Substitute.For<IProjectSearchResult>();
    var expectedResult = Any.Seq<IDependencyPathBasedRuleTarget>();
    var projectPath = Any.Seq<IDependencyPathBasedRuleTarget>();

    anotherResult.TerminatedSegmentStartingFrom(resultOccurenceIndex, projectPath).Returns(expectedResult);

    //WHEN
    var segment = result.SegmentEndingWith(anotherResult, projectPath);

    //THEN
    segment.Should().BeEquivalentTo(expectedResult);
  }

  [Fact]
  public void ShouldReturnTerminatedSegmentUsingPassedStartIndex()
  {
    //GIVEN
    var project1 = Any.Instance<IDependencyPathBasedRuleTarget>();
    var project2 = Any.Instance<IDependencyPathBasedRuleTarget>();
    var project3 = Any.Instance<IDependencyPathBasedRuleTarget>();
    var project4 = Any.Instance<IDependencyPathBasedRuleTarget>();
    var projectPath = Seq.create(project1, project2, project3, project4);
    const int startIndex = 1;
    const int endIndex = 2;
    var result = new ProjectFoundSearchResult(Any.Instance<IDependencyPathBasedRuleTarget>(), endIndex);

    //WHEN
    var segment = result.TerminatedSegmentStartingFrom(startIndex, projectPath);

    //THEN
    segment.Should().BeEquivalentTo(new []{ project2, project3 });
  }

  [Fact]
  public void ShouldNotBeAnotherProject()
  {
    //GIVEN
    var project = Any.Instance<IDependencyPathBasedRuleTarget>();
    var searchResult = new ProjectFoundSearchResult(project, Any.Integer());

    //WHEN
    var isNotAnotherProject = searchResult.IsNot(Any.OtherThan(project));

    //THEN
    isNotAnotherProject.Should().BeTrue();
  }

  [Fact]
  public void ShouldBeItself()
  {
    //GIVEN
    var project = Any.Instance<IDependencyPathBasedRuleTarget>();
    var searchResult = new ProjectFoundSearchResult(project, Any.Integer());

    //WHEN
    var isNotItself = searchResult.IsNot(project);

    //THEN
    isNotItself.Should().BeFalse();
  }


  [Fact]
  public void ShouldBeBeforeHigherIndex()
  {
    //GIVEN
    var occurenceIndex = Any.Integer();
    var searchResult = new ProjectFoundSearchResult(Any.Instance<IDependencyPathBasedRuleTarget>(), occurenceIndex);

    //WHEN
    var isBefore = searchResult.IsNotAfter(occurenceIndex + 1);

    //THEN
    isBefore.Should().BeTrue();
  }

  [Fact]
  public void ShouldSayItIsNotAfterItsIndex()
  {
    //GIVEN
    var occurenceIndex = Any.Integer();
    var searchResult = new ProjectFoundSearchResult(
      Any.Instance<IDependencyPathBasedRuleTarget>(),
      occurenceIndex);

    //WHEN
    var isBefore = searchResult.IsNotAfter(occurenceIndex);

    //THEN
    isBefore.Should().BeTrue();
  }
}
