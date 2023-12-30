using System;
using System.Collections.Generic;
using AtmaFileSystem;
using LanguageExt;
using NScan.SharedKernel.ReadingCSharpSourceCode;

namespace NScan.SharedKernel.ReadingSolution.Ports;

public class SourceCodeFileDto(
  RelativeFilePath filePathRelativeToProjectRoot,
  Seq<string> declaredNamespaces,
  string parentProjectRootNamespace,
  string parentProjectAssemblyName,
  Seq<string> usings,
  IReadOnlyList<ClassDeclarationInfo> classes)
{
  public IReadOnlyList<ClassDeclarationInfo> Classes { get; } = classes ?? throw new ArgumentNullException(nameof(classes));
  public Seq<string> Usings { get; } = usings;
  public string ParentProjectAssemblyName { get; } = parentProjectAssemblyName ?? throw new ArgumentNullException(nameof(parentProjectAssemblyName));
  public string ParentProjectRootNamespace { get; } = parentProjectRootNamespace ?? throw new ArgumentNullException(nameof(parentProjectRootNamespace));
  public RelativeFilePath PathRelativeToProjectRoot { get; } = filePathRelativeToProjectRoot;
  public Seq<string> DeclaredNamespaces { get; } = declaredNamespaces;
}
