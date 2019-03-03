using FluentAssertions;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.XFluentAssert.Root;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.SharedKernel
{
  public class PackageReferenceSpecification
  {
    [Fact]
    public void ShouldBehaveLikeValueObject()
    {
      typeof(PackageReference).Should().HaveValueSemantics();
    }

    [Fact]
    public void ShouldGiveNameAndVersionWhenConvertedToString()
    {
      //GIVEN
      var name = Any.Instance<string>();
      var version = Any.Instance<string>();
      var packageReference = new PackageReference(name, version);
      
      //WHEN
      var stringRepresentation = packageReference.ToString();

      //THEN
      stringRepresentation.Should().Be($"{name}, Version {version}");
    }
  }
}