using LanguageExt;

namespace NScan.SharedKernel.ReadingSolution.Ports;

public sealed record CsharpProjectDto(
  ProjectId Id,
  string AssemblyName,
  Seq<SourceCodeFileDto> SourceCodeFiles,
  HashMap<string, string> Properties,
  Seq<PackageReference> PackageReferences,
  Seq<AssemblyReference> AssemblyReferences,
  Seq<ProjectId> ReferencedProjectIds,
  Seq<string> TargetFrameworks);
