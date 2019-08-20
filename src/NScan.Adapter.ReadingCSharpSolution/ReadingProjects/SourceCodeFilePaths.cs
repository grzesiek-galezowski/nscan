using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AtmaFileSystem;
using NScan.Adapter.ReadingCSharpSolution.ReadingCSharpSourceCode;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using NScan.SharedKernel.ReadingSolution.Lib;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.Adapter.ReadingCSharpSolution.ReadingProjects
{
  public static class SourceCodeFilePaths
  {
    public static IEnumerable<AbsoluteFilePath> SourceCodeFilesIn(AbsoluteDirectoryPath projectDirectory)
    {
      return Directory.EnumerateFiles(
          projectDirectory.ToString(), "*.cs", SearchOption.AllDirectories)
        .Select(AtmaFileSystemPaths.AbsoluteFilePath)
        .Where(IsNotInDirectory(projectDirectory, "obj"));
    }

    public static void LoadFilesInto(XmlProjectDataAccess projectAccess)
    {
      var projectDirectory = projectAccess.GetParentDirectoryName();

      var syntaxTrees = SourceCodeFilesIn(projectDirectory).Select(CSharpFileSyntaxTree.ParseFile).ToArray();

      var classDeclarationSignatures
        = CSharpFileSyntaxTree.GetClassDeclarationSignaturesFromFiles(syntaxTrees);

      foreach (var dotNetProject 
        in syntaxTrees.Select(tree => CreateXmlSourceCodeFile(projectAccess, projectDirectory, tree, classDeclarationSignatures)))
      {
        projectAccess.AddFile(dotNetProject);
      }
    }

    private static Func<AbsoluteFilePath, bool> IsNotInDirectory(AbsoluteDirectoryPath projectDirectory, string dirName)
    {
      return f => !GetPathRelativeTo(projectDirectory, f).StartsWith(dirName + Path.DirectorySeparatorChar);
    }

    private static string GetPathRelativeTo(AbsoluteDirectoryPath projectDirectory, AbsoluteFilePath file)
    {
      return file.ToString().Replace(projectDirectory.ToString() + Path.DirectorySeparatorChar, "");
    }


    private static SourceCodeFileDto CreateXmlSourceCodeFile(
      XmlProjectDataAccess projectAccess, 
      AbsoluteDirectoryPath projectDirectory, 
      ICSharpFileSyntaxTree syntaxTree, 
      Dictionary<string, ClassDeclarationInfo> classDeclarationSignatures)
    {
      return new SourceCodeFileDto(
        AtmaFileSystemPaths.RelativeFilePath(GetPathRelativeTo(projectDirectory, syntaxTree.FilePath)), 
        syntaxTree.GetAllUniqueNamespaces().ToList(), 
        projectAccess.RootNamespace(), 
        projectAccess.DetermineAssemblyName(), 
        syntaxTree.GetAllUsingsFrom(classDeclarationSignatures),
        classDeclarationSignatures.Values.ToList());
    }
  }
}