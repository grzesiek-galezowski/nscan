using System.Collections.Generic;
using System.Collections.Immutable;

namespace NScan.SharedKernel.ReadingSolution.Ports
{
  public record CsharpProjectDto
  {
    public CsharpProjectDto(ProjectId projectId,
      string assemblyName,
      string targetFramework,
      ImmutableList<SourceCodeFileDto> sourceCodeFileDtos,
      ImmutableDictionary<string, string> properties,
      ImmutableList<PackageReference> packageReferences,
      ImmutableList<AssemblyReference> assemblyReferences,
      ImmutableList<ProjectId> referencedProjectIds)
    {
      AssemblyName = assemblyName;
      SourceCodeFiles = sourceCodeFileDtos;
      TargetFramework = targetFramework;
      Id = projectId;
      PackageReferences = packageReferences;
      AssemblyReferences = assemblyReferences;
      ReferencedProjectIds = referencedProjectIds;
      Properties = properties;
    }

    public string AssemblyName { get; }

    public ProjectId Id { get; }

    public IEnumerable<SourceCodeFileDto> SourceCodeFiles { get; }

    public string TargetFramework { get; }

    public IReadOnlyList<PackageReference> PackageReferences { get; }

    public IReadOnlyList<AssemblyReference> AssemblyReferences { get; }

    public IReadOnlyList<ProjectId> ReferencedProjectIds { get; }

    public IReadOnlyDictionary<string, string> Properties { get; }
  }
}
