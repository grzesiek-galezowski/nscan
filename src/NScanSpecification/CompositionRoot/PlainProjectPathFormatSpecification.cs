using System.Collections.Generic;
using FluentAssertions;
using TddXt.NScan.CompositionRoot;
using TddXt.NScan.Domain;
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
      var format = new PlainReportFragmentsFormat();
      var p1 = Any.Instance<IReferencedProject>();
      var p2 = Any.Instance<IReferencedProject>();
      var p3 = Any.Instance<IReferencedProject>();

      //WHEN
      var result = format.ApplyToPath(new List<IReferencedProject>() {p1, p2, p3});

      //THEN
      result.Should().Be($"[{p1.ToString()}]->[{p2.ToString()}]->[{p3.ToString()}]");
    }

    [Fact]
    public void ShouldCreateStringWithSingleProjectWhenViolationPathConsistsOfASingleProject()
    {
      //GIVEN
      var format = new PlainReportFragmentsFormat();
      var p1 = Any.Instance<IReferencedProject>();

      //WHEN
      var result = format.ApplyToPath(new List<IReferencedProject>() {p1});

      //THEN
      result.Should().Be($"[{p1.ToString()}]");
    }
  }
}