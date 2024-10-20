﻿using System.IO;
using System.Linq;
using AtmaFileSystem;
using LanguageExt;
using Microsoft.Build.Construction;
using NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingCSharpSourceCode;
using NScan.SharedKernel.ReadingSolution.Ports;
using Project = Microsoft.Build.Evaluation.Project;
using ProjectId = NScan.SharedKernel.ProjectId;

namespace NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingProjects;

public class MsBuildProject
{
  private readonly Project _project;

  public static MsBuildProject From(AbsoluteFilePath projectFilePath)
  {
    var project = new Project(ProjectRootElement.Open(projectFilePath.ToString()));
    var msBuildProject = new MsBuildProject(project);
    return msBuildProject;
  }

  private MsBuildProject(Project project)
  {
    _project = project;
  }

  public Seq<ProjectId> ProjectReferences()
  {
    return _project.AllEvaluatedItems.Where(item => item.ItemType == "ProjectReference")
      .Select(item => new ProjectId((FullPath.ParentDirectory() + AtmaFileSystemPaths.RelativeDirectoryPath(item.EvaluatedInclude)).ToString()))
      .ToSeq();
  }

  public Seq<AssemblyReference> AssemblyReferences()
  {
    return _project.Items.Where(item => item.ItemType == "AssemblyReference")
      .Select(item => new AssemblyReference(item.EvaluatedInclude, item.GetMetadata("HintPath").EvaluatedValue))
      .ToSeq();
  }

  public Seq<PackageReference> PackageReferences()
  {
    return _project.Items
      .Where(item => item.ItemType == "PackageReference")
      .Where(item => (!item.HasMetadata("IsImplicitlyDefined")) || (item.GetMetadataValue("IsImplicitlyDefined") == "false")) //to filter out .net sdk dependency
      .Select(item =>
        new PackageReference(item.EvaluatedInclude, item.GetMetadataValue("Version")))
      .ToSeq();
  }

  public HashMap<string, string> Properties()
  {
    return _project.Properties.ToDictionary(p => p.Name, p => p.EvaluatedValue).ToHashMap();
  }

  public Seq<string> TargetFrameworks()
  {
    var value = (_project.Properties.FirstOrDefault(p => p.Name == "TargetFrameworks") ??
     _project.Properties.Single(p => p.Name == "TargetFramework")).EvaluatedValue;
    return value.Split(";").ToSeq();
  }

  public string AssemblyName()
  {
    return _project.Properties.Single(p => p.Name == "AssemblyName").EvaluatedValue;
  }

  public ProjectId Id()
  {
    return new ProjectId(_project.FullPath);
  }

  public Seq<SourceCodeFileDto> LoadSourceCodeFiles()
  {
    var csprojRoot = FullPath.ParentDirectory();
    var syntaxTrees = AllCompiledFilesPaths(csprojRoot);

    var classDeclarationSignatures
      = CSharpFileSyntaxTree.GetClassDeclarationSignaturesFromFiles(syntaxTrees);

    return syntaxTrees.Select(tree => 
        SourceCodeFilePaths.CreateSourceCodeFileDto(
          csprojRoot, 
          tree, 
          classDeclarationSignatures, 
          RootNamespace(), 
          AssemblyName()))
      .ToSeq();
  }

  public string RootNamespace()
  {
    return _project.Properties.Single(p => p.Name == "RootNamespace").EvaluatedValue;
  }

  private AbsoluteFilePath FullPath => AbsoluteFilePath.Value(_project.FullPath);

  private Seq<CSharpFileSyntaxTree> AllCompiledFilesPaths(AbsoluteDirectoryPath csprojRoot)
  {
    return _project.Items
      .Where(item => item.ItemType == "Compile")
 
      //some file paths, e.g. added by external nuget packages, are outside the project,
      //and so should not be analyzed, e.g.
      //C:\Users\<USER>\.nuget\packages\isexternalinit\1.0.3\contentFiles\cs\net5.0\IsExternalInit\IsExternalInit.cs
      .Where(p => !Path.IsPathFullyQualified(p.EvaluatedInclude))
      .Select(p => csprojRoot + AtmaFileSystemPaths.RelativeFilePath(p.EvaluatedInclude))
      .Select(CSharpFileSyntaxTree.ParseFile).ToSeq();
  }
}
