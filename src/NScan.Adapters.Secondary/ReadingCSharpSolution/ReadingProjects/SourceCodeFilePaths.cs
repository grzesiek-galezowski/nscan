using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using AtmaFileSystem;
using Microsoft.Build.Evaluation;
using NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingCSharpSourceCode;
using NScan.SharedKernel.ReadingCSharpSourceCode;
using NScan.SharedKernel.ReadingSolution.Ports;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects;

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

    foreach (var file 
             in syntaxTrees.Select(tree => CreateXmlSourceCodeFile(projectAccess, projectDirectory, tree, classDeclarationSignatures)))
    {
      projectAccess.AddFile(file);
    }
  }

  public static ImmutableList<SourceCodeFileDto> LoadFiles(Project project, AbsoluteDirectoryPath csprojRoot)
  {
    var syntaxTrees = project.Items.Where(item => item.ItemType == "Compile")
      .Select(p => csprojRoot + AtmaFileSystemPaths.RelativeFilePath(p.EvaluatedInclude))
      .Select(CSharpFileSyntaxTree.ParseFile).ToArray();

    var classDeclarationSignatures
      = CSharpFileSyntaxTree.GetClassDeclarationSignaturesFromFiles(syntaxTrees);

    return syntaxTrees.Select(tree => 
        CreateXmlSourceCodeFile2(project, csprojRoot, tree, classDeclarationSignatures))
      .ToImmutableList();
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
  
  private static SourceCodeFileDto CreateXmlSourceCodeFile2(
    Project project, 
    AbsoluteDirectoryPath projectDirectory, 
    ICSharpFileSyntaxTree syntaxTree, 
    Dictionary<string, ClassDeclarationInfo> classDeclarationSignatures)
  {
    return new SourceCodeFileDto(
      AtmaFileSystemPaths.RelativeFilePath(GetPathRelativeTo(projectDirectory, syntaxTree.FilePath)), 
      syntaxTree.GetAllUniqueNamespaces().ToList(), 
      project.Properties.Single(p => p.Name == "RootNamespace").EvaluatedValue, //bug wrap this class
      project.Properties.Single(p => p.Name == "AssemblyName").EvaluatedValue, //bug wrap this class
      syntaxTree.GetAllUsingsFrom(classDeclarationSignatures),
      classDeclarationSignatures.Values.ToList());
  }
}
