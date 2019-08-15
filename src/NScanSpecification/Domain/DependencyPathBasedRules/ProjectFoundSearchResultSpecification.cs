using System.Collections.Generic;
using FluentAssertions;
using NScan.Domain.Domain.DependencyPathBasedRules;
using NScan.Domain.Domain.Root;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Numbers;
using Xunit;

namespace TddXt.NScan.Specification.Domain.DependencyPathBasedRules
{
  public class ProjectFoundSearchResultSpecification
  {
    [Fact]
    public void ShouldExistWhenWrappingNonNullInstance()
    {
      //GIVEN
      var result = new ProjectFoundSearchResult(AnyRoot.Root.Any.Instance<IReferencedProject>(), AnyRoot.Root.Any.Integer());

      //WHEN
      var exists = result.Exists();

      //THEN
      exists.Should().BeTrue();
    }

    [Fact]
    public void ShouldExistAfterAnotherResultWhenItExistsAndAnotherResultIsBeforeItsccurenceIndex()
    {
      //GIVEN
      var resultOccurenceIndex = AnyRoot.Root.Any.Integer();
      var result = new ProjectFoundSearchResult(AnyRoot.Root.Any.Instance<IReferencedProject>(), resultOccurenceIndex);
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
      var resultOccurenceIndex = AnyRoot.Root.Any.Integer();
      var result = new ProjectFoundSearchResult(AnyRoot.Root.Any.Instance<IReferencedProject>(), resultOccurenceIndex);
      var anotherResult = Substitute.For<IProjectSearchResult>();
      var expectedResult = AnyRoot.Root.Any.ReadOnlyList<IReferencedProject>();
      var projectPath = AnyRoot.Root.Any.Enumerable<IReferencedProject>();

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
      var project1 = AnyRoot.Root.Any.Instance<IReferencedProject>();
      var project2 = AnyRoot.Root.Any.Instance<IReferencedProject>();
      var project3 = AnyRoot.Root.Any.Instance<IReferencedProject>();
      var project4 = AnyRoot.Root.Any.Instance<IReferencedProject>();
      var projectPath = new List<IDependencyPathBasedRuleTarget>
      {
        project1,
        project2,
        project3,
        project4
      };
      var startIndex = 1;
      var endIndex = 2;
      var result = new ProjectFoundSearchResult(AnyRoot.Root.Any.Instance<IReferencedProject>(), endIndex);

      //WHEN
      var segment = result.TerminatedSegmentStartingFrom(startIndex, projectPath);

      //THEN
      segment.Should().BeEquivalentTo(project2, project3);
    }


    [Fact]
    public void ShouldNotBeAnotherProject()
    {
      //GIVEN
      var project = AnyRoot.Root.Any.Instance<IReferencedProject>();
      var searchResult = new ProjectFoundSearchResult(project, AnyRoot.Root.Any.Integer());

      //WHEN
      var isNotAnotherProject = searchResult.IsNot(AnyRoot.Root.Any.OtherThan(project));

      //THEN
      isNotAnotherProject.Should().BeTrue();
    }

    [Fact]
    public void ShouldBeItself()
    {
      //GIVEN
      var project = AnyRoot.Root.Any.Instance<IReferencedProject>();
      var searchResult = new ProjectFoundSearchResult(project, AnyRoot.Root.Any.Integer());

      //WHEN
      var isNotItself = searchResult.IsNot(project);

      //THEN
      isNotItself.Should().BeFalse();
    }


    [Fact]
    public void ShouldBeBeforeHigherIndex()
    {
      //GIVEN
      var occurenceIndex = AnyRoot.Root.Any.Integer();
      var searchResult = new ProjectFoundSearchResult(AnyRoot.Root.Any.Instance<IReferencedProject>(), occurenceIndex);

      //WHEN
      var isBefore = searchResult.IsNotAfter(occurenceIndex + 1);

      //THEN
      isBefore.Should().BeTrue();
    }

    [Fact]
    public void ShouldSayItIsNotAfterItsIndex()
    {
      //GIVEN
      var occurenceIndex = AnyRoot.Root.Any.Integer();
      var searchResult = new ProjectFoundSearchResult(
        AnyRoot.Root.Any.Instance<IReferencedProject>(),
        occurenceIndex);

      //WHEN
      var isBefore = searchResult.IsNotAfter(occurenceIndex);

      //THEN
      isBefore.Should().BeTrue();
    }
  }
}