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
  Seq<ClassDeclarationInfo> classes)
{
  public Seq<ClassDeclarationInfo> Classes { get; } = classes;
  public Seq<string> Usings { get; } = usings;
  public string ParentProjectAssemblyName { get; } = parentProjectAssemblyName;
  public string ParentProjectRootNamespace { get; } = parentProjectRootNamespace;
  public RelativeFilePath PathRelativeToProjectRoot { get; } = filePathRelativeToProjectRoot;
  public Seq<string> DeclaredNamespaces { get; } = declaredNamespaces;
}
