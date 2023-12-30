using AtmaFileSystem;
using LanguageExt;
using NScan.SharedKernel.ReadingCSharpSourceCode;

namespace NScan.SharedKernel.ReadingSolution.Ports;

public record SourceCodeFileDto(
  RelativeFilePath PathRelativeToProjectRoot,
  Seq<string> DeclaredNamespaces,
  string ParentProjectRootNamespace,
  string ParentProjectAssemblyName,
  Seq<string> Usings,
  Seq<ClassDeclarationInfo> Classes);
