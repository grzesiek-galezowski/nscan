using System.Collections.Generic;
using System.IO;
using System.Linq;
using AtmaFileSystem;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects;

public static class SourceCodeFilePaths
{
  public static SourceCodeFileDto CreateSourceCodeFileDto(
    AbsoluteDirectoryPath projectDirectory, 
    ICSharpFileSyntaxTree syntaxTree, 
    Dictionary<string, ClassDeclarationInfo> classDeclarationSignatures, 
    string parentProjectRootNamespace, 
    string parentProjectAssemblyName)
  {
    return new SourceCodeFileDto(
      AtmaFileSystemPaths.RelativeFilePath(GetPathRelativeTo(projectDirectory, syntaxTree.FilePath)), 
      syntaxTree.GetAllUniqueNamespaces().ToList(), 
      parentProjectRootNamespace, 
      parentProjectAssemblyName,
      syntaxTree.GetAllUsingsFrom(classDeclarationSignatures),
      classDeclarationSignatures.Values.ToList());
  }

  private static string GetPathRelativeTo(AbsoluteDirectoryPath projectDirectory, AbsoluteFilePath file)
  {
    return file.ToString().Replace(projectDirectory.ToString() + Path.DirectorySeparatorChar, "");
  }
}
