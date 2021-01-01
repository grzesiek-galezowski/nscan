using System.Collections.Generic;

namespace NScan.SharedKernel.ReadingSolution.Ports
{
  public class CsharpProjectDto
  {
    public CsharpProjectDto(
      ProjectId projectId, 
      string assemblyName, 
      string targetFramework,
      IEnumerable<SourceCodeFileDto> sourceCodeFileDtos, 
      IReadOnlyList<PackageReference> packageReferences,
      IReadOnlyList<AssemblyReference> assemblyReferences, 
      IReadOnlyList<ProjectId> referencedProjectIds)
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

    public IReadOnlyList<ProjectId> ReferencedProjectIds { get; }
  }
}
