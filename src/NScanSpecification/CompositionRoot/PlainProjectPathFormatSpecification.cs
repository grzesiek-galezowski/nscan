using System.Collections.Generic;
using FluentAssertions;
using TddXt.NScan.App;
using TddXt.NScan.CompositionRoot;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.CompositionRoot
{
  public class PlainProjectPathFormatSpecification
  {
    [Fact]
    public void ShouldCreateStringWithConcatenatedStringRepresentationOfProjectPathWhenApplied()
    {
      //GIVEN
      var format = new PlainProjectPathFormat();
      var p1 = Any.Instance<IReferencedProject>();
      var p2 = Any.Instance<IReferencedProject>();
      var p3 = Any.Instance<IReferencedProject>();

      //WHEN
      var result = format.ApplyTo(new List<IReferencedProject>() {p1, p2, p3});

      //THEN
      result.Should().Be($"[{p1.ToString()}]->[{p2.ToString()}]->[{p3.ToString()}]");
    }

    [Fact]
    public void ShouldCreateStringWithSingleProjectWhenViolationPathConsistsOfASingleProject()
    {
      //GIVEN
      var format = new PlainProjectPathFormat();
      var p1 = Any.Instance<IReferencedProject>();

      //WHEN
      var result = format.ApplyTo(new List<IReferencedProject>() {p1});

      //THEN
      result.Should().Be($"[{p1.ToString()}]");
    }
  }
}