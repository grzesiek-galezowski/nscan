using System.Collections.Generic;
using FluentAssertions;
using TddXt.AnyRoot;
using TddXt.NScan.App;
using TddXt.NScan.CompositionRoot;
using Xunit;

namespace TddXt.NScan.Specification.CompositionRoot
{
  public class PlainProjectPathFormatSpecification
  {
    [Fact]
    public void ShouldCreateStringWithConcatenatedStringRepresentationOfProjectPathWhenApplied()
    {
      //GIVEN
      var format = new PlainProjectPathFormat();
      var p1 = Root.Any.Instance<IReferencedProject>();
      var p2 = Root.Any.Instance<IReferencedProject>();
      var p3 = Root.Any.Instance<IReferencedProject>();

      //WHEN
      var result = format.ApplyTo(new List<IReferencedProject>() {p1, p2, p3});

      //THEN
      result.Should().Be($"[{p1.ToString()}]->[{p2.ToString()}]->[{p3.ToString()}]");
    }
  }
}