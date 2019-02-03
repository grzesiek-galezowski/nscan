using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Numbers;
using TddXt.NScan.Domain;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.SharedKernel;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain
{
  public class ProjectFoundSearchResultSpecification
  {
    [Fact]
    public void ShouldExistWhenWrappingNonNullInstance()
    {
      //GIVEN
      var result = new ProjectFoundSearchResult(Any.Instance<IReferencedProject>(), Any.Integer());

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
      var result = new ProjectFoundSearchResult(Any.Instance<IReferencedProject>(), resultOccurenceIndex);
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
      var result = new ProjectFoundSearchResult(Any.Instance<IReferencedProject>(), resultOccurenceIndex);
      var anotherResult = Substitute.For<IProjectSearchResult>();
      var expectedResult = Any.ReadOnlyList<IReferencedProject>();
      var projectPath = Any.Enumerable<IReferencedProject>();

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
      var project1 = Any.Instance<IReferencedProject>();
      var project2 = Any.Instance<IReferencedProject>();
      var project3 = Any.Instance<IReferencedProject>();
      var project4 = Any.Instance<IReferencedProject>();
      var projectPath = new List<IDependencyPathBasedRuleTarget>
      {
        project1,
        project2,
        project3,
        project4
      };
      var startIndex = 1;
      var endIndex = 2;
      var result = new ProjectFoundSearchResult(Any.Instance<IReferencedProject>(), endIndex);

      //WHEN
      var segment = result.TerminatedSegmentStartingFrom(startIndex, projectPath);

      //THEN
      segment.Should().BeEquivalentTo(project2, project3);
    }


    [Fact]
    public void ShouldNotBeAnotherProject()
    {
      //GIVEN
      var project = Any.Instance<IReferencedProject>();
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
      var project = Any.Instance<IReferencedProject>();
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
      var searchResult = new ProjectFoundSearchResult(Any.Instance<IReferencedProject>(), occurenceIndex);

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
        Any.Instance<IReferencedProject>(),
        occurenceIndex);

      //WHEN
      var isBefore = searchResult.IsNotAfter(occurenceIndex);

      //THEN
      isBefore.Should().BeTrue();
    }
  }
}