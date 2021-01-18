using FluentAssertions;
using NScan.SharedKernel.ReadingSolution.Ports;
using Xunit;
using static TddXt.AnyRoot.Root;

namespace NScanSpecification.Domain.SharedKernel
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
