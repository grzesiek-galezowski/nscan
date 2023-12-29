using System.Collections.Immutable;
using LanguageExt;

namespace NScan.SharedKernel.ReadingSolution.Ports;

public sealed record CsharpProjectDto(
  ProjectId Id,
  string AssemblyName,
  Arr<SourceCodeFileDto> SourceCodeFiles,
  Map<string, string> Properties,
  Arr<PackageReference> PackageReferences,
  Arr<AssemblyReference> AssemblyReferences,
  Arr<ProjectId> ReferencedProjectIds,
  Arr<string> TargetFrameworks);
