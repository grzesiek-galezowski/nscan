using System.Collections.Generic;
using NScan.Domain.Root;

namespace NScan.SharedKernel.ReadingSolution.Ports
{
  public class CsharpProjectDto
  {
    public CsharpProjectDto(string assemblyName, IEnumerable<SourceCodeFileDto> sourceCodeFileDtos, string targetFramework, ProjectId projectId, List<PackageReference> packageReferences, List<AssemblyReference> assemblyReferences, ProjectId[] referencedProjectIds)
    {
      AssemblyName = assemblyName;
      SourceCodeFiles = sourceCodeFileDtos;
      TargetFramework = targetFramework;
      Id = projectId;
      PackageReferences = packageReferences;
      AssemblyReferences = assemblyReferences;
      ReferencedProjectIds = referencedProjectIds;
    }

    public string AssemblyName { get; }

    public ProjectId Id { get; }

    public IEnumerable<SourceCodeFileDto> SourceCodeFiles { get; }

    public string TargetFramework { get; }

    public IReadOnlyList<PackageReference> PackageReferences { get; }

    public IReadOnlyList<AssemblyReference> AssemblyReferences { get; }

    public ProjectId[] ReferencedProjectIds { get; }
  }
}