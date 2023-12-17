using System;
using System.Collections.Generic;
using AtmaFileSystem;
using NScan.SharedKernel.ReadingCSharpSourceCode;

namespace NScan.SharedKernel.ReadingSolution.Ports;

public class SourceCodeFileDto(
  RelativeFilePath filePathRelativeToProjectRoot,
  List<string> declaredNamespaces,
  string parentProjectRootNamespace,
  string parentProjectAssemblyName,
  IReadOnlyList<string> usings,
  IReadOnlyList<ClassDeclarationInfo> classes)
{
  public IReadOnlyList<ClassDeclarationInfo> Classes { get; } = classes ?? throw new ArgumentException(nameof(classes));
  public IReadOnlyList<string> Usings { get; } = usings ?? throw new ArgumentException(nameof(usings));
  public string ParentProjectAssemblyName { get; } = parentProjectAssemblyName ?? throw new ArgumentNullException(nameof(parentProjectAssemblyName));
  public string ParentProjectRootNamespace { get; } = parentProjectRootNamespace ?? throw new ArgumentNullException(nameof(parentProjectRootNamespace));
  public RelativeFilePath PathRelativeToProjectRoot { get; } = filePathRelativeToProjectRoot;
  public IReadOnlyList<string> DeclaredNamespaces { get; } = declaredNamespaces ?? throw new ArgumentException(nameof(declaredNamespaces));
}
