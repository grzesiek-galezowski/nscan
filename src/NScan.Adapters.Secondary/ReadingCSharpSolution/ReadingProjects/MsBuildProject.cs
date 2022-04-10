using System.Collections.Immutable;
using System.Linq;
using AtmaFileSystem;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using NScan.Adapters.Secondary.ReadingCSharpSolution.ReadingCSharpSourceCode;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingSolution.Ports;

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

  public ImmutableList<ProjectId> ProjectReferences()
  {
    return _project.AllEvaluatedItems.Where(item => item.ItemType == "ProjectReference")
      .Select(item => new ProjectId((FullPath.ParentDirectory() + AtmaFileSystemPaths.RelativeDirectoryPath(item.EvaluatedInclude)).ToString()))
      .ToImmutableList();
  }

  public ImmutableList<AssemblyReference> AssemblyReferences()
  {
    return _project.Items.Where(item => item.ItemType == "AssemblyReference")
      .Select(item => new AssemblyReference(item.EvaluatedInclude, item.GetMetadata("HintPath").EvaluatedValue))
      .ToImmutableList();
  }

  public ImmutableList<PackageReference> PackageReferences()
  {
    return _project.Items
      .Where(item => item.ItemType == "PackageReference")
      .Where(item => (!item.HasMetadata("IsImplicitlyDefined")) || (item.GetMetadataValue("IsImplicitlyDefined") == "false")) //to filter out .net sdk dependency
      .Select(item =>
        new PackageReference(item.EvaluatedInclude, item.Metadata.Single(m => m.Name == "Version").EvaluatedValue))
      .ToImmutableList();
  }

  public ImmutableDictionary<string, string> Properties()
  {
    return _project.Properties.ToDictionary(p => p.Name, p => p.EvaluatedValue).ToImmutableDictionary();
  }

  public string TargetFramework()
  {
    return _project.Properties.Single(p => p.Name == "TargetFramework").EvaluatedValue;
  }

  public string AssemblyName()
  {
    return _project.Properties.Single(p => p.Name == "AssemblyName").EvaluatedValue;
  }

  public ProjectId Id()
  {
    return new ProjectId(_project.FullPath);
  }

  public ImmutableList<SourceCodeFileDto> LoadSourceCodeFiles()
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
      .ToImmutableList();
  }

  public string RootNamespace()
  {
    return _project.Properties.Single(p => p.Name == "RootNamespace").EvaluatedValue;
  }

  private AbsoluteFilePath FullPath => AbsoluteFilePath.Value(_project.FullPath);

  private CSharpFileSyntaxTree[] AllCompiledFilesPaths(AbsoluteDirectoryPath csprojRoot)
  {
    return _project.Items.Where(item => item.ItemType == "Compile")
      .Select(p => csprojRoot + AtmaFileSystemPaths.RelativeFilePath(p.EvaluatedInclude))
      .Select(CSharpFileSyntaxTree.ParseFile).ToArray();
  }
}
