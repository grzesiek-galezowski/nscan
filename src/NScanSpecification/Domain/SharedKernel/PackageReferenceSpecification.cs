using FluentAssertions;
using NScan.SharedKernel;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace TddXt.NScan.Specification.Domain.SharedKernel
{
  public class PackageReferenceSpecification
  {

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