using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.SharedKernelSpecification.ReadingSolution.Ports;

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
