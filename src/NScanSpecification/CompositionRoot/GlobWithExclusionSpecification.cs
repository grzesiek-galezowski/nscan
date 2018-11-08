using FluentAssertions;
using TddXt.NScan.CompositionRoot;
using TddXt.XFluentAssert.Root;
using Xunit;

namespace TddXt.NScan.Specification.CompositionRoot
{
  public class GlobWithExclusionSpecification
  {
    [Fact]
    public void ShouldBehaveLikeValueObject()
    {
      typeof(GlobWithExclusion).Should().HaveValueSemantics();
    }
  }
}