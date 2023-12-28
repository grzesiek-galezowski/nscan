using System.Collections.Immutable;

namespace NScan.SharedKernel.ReadingSolution.Ports;

public sealed record CsharpProjectDto(
  ProjectId Id,
  string AssemblyName,
  ImmutableList<SourceCodeFileDto> SourceCodeFiles,
  ImmutableDictionary<string, string> Properties,
  ImmutableList<PackageReference> PackageReferences,
  ImmutableList<AssemblyReference> AssemblyReferences,
  ImmutableList<ProjectId> ReferencedProjectIds,
  ImmutableList<string> TargetFrameworks);
