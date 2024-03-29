﻿using System.IO;
using AtmaFileSystem;
using LanguageExt;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects;

public static class SourceCodeFilePaths
{
  public static SourceCodeFileDto CreateSourceCodeFileDto(
    AbsoluteDirectoryPath projectDirectory, 
    ICSharpFileSyntaxTree syntaxTree, 
    HashMap<string, ClassDeclarationInfo> classDeclarationSignatures, 
    string parentProjectRootNamespace, 
    string parentProjectAssemblyName)
  {
    return new SourceCodeFileDto(
      AtmaFileSystemPaths.RelativeFilePath(GetPathRelativeTo(projectDirectory, syntaxTree.FilePath)), 
      syntaxTree.GetAllUniqueNamespaces(), 
      parentProjectRootNamespace, 
      parentProjectAssemblyName,
      syntaxTree.GetAllUsingsFrom(classDeclarationSignatures),
      classDeclarationSignatures.Values.ToSeq());
  }

  private static string GetPathRelativeTo(AbsoluteDirectoryPath projectDirectory, AbsoluteFilePath file)
  {
    return file.ToString().Replace(projectDirectory.ToString() + Path.DirectorySeparatorChar, "");
  }
}
