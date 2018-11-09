using FluentAssertions;
using TddXt.NScan.CompositionRoot;
using TddXt.XFluentAssert.Root;
using Xunit;

namespace TddXt.NScan.Specification.CompositionRoot
{
  public class PatternSpecification
  {
    [Fact]
    public void ShouldBehaveLikeValueObject()
    {
      typeof(Pattern).Should().HaveValueSemantics();
    }
  }
}