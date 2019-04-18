﻿using System;
using System.Collections.Generic;
using AtmaFileSystem;
using TddXt.NScan.ReadingCSharpSourceCode;

namespace TddXt.NScan.ReadingSolution.Ports
{
  public class XmlSourceCodeFile
  {

    public XmlSourceCodeFile(RelativeFilePath filePathRelativeToProjectRoot,
      List<string> declaredNamespaces,
      string parentProjectRootNamespace,
      string parentProjectAssemblyName,
      IReadOnlyList<string> usings, 
      IReadOnlyList<ClassDeclarationInfo> classes)
    {
      Classes = classes ?? throw new ArgumentException(nameof(classes));
      Usings = usings ?? throw new ArgumentException(nameof(usings));
      PathRelativeToProjectRoot = filePathRelativeToProjectRoot;
      DeclaredNamespaces = declaredNamespaces ?? throw new ArgumentException(nameof(declaredNamespaces));
      ParentProjectRootNamespace = parentProjectRootNamespace ?? throw new ArgumentNullException(nameof(parentProjectRootNamespace));
      ParentProjectAssemblyName = parentProjectAssemblyName ?? throw new ArgumentNullException(nameof(parentProjectAssemblyName));
    }

    public IReadOnlyList<ClassDeclarationInfo> Classes { get; }
    public IReadOnlyList<string> Usings { get; }
    public string ParentProjectAssemblyName { get; }
    public string ParentProjectRootNamespace { get; }
    public RelativeFilePath PathRelativeToProjectRoot { get; }
    public IReadOnlyList<string> DeclaredNamespaces { get; }
  }
}