using System.Collections.Generic;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.Domain.Root
{
  public interface IXmlProjectDataAccess2
  {
    string AssemblyName { get; }
    ProjectId Id { get; }
    IEnumerable<SourceCodeFileDto> SourceCodeFiles { get; }
    string TargetFramework { get; }
    List<PackageReference> PackageReferences { get; }
    List<AssemblyReference> AssemblyReferences { get; }
    ProjectId[] ProjectIds { get; }
  }
}