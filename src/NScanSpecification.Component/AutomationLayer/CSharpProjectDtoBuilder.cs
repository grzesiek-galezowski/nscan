using System.IO;
using System.Linq;
using AtmaFileSystem;
using LanguageExt;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScanSpecification.Lib;
using static AtmaFileSystem.AtmaFileSystemPaths;

namespace NScanSpecification.Component.AutomationLayer;

public class CSharpProjectDtoBuilder //bug make this a record
{
  private readonly string _assemblyName;
  private string _targetFramework;
  private Seq<SourceCodeFileDtoBuilder> _sourceCodeFileBuilders;
  private readonly ProjectId _projectId;
  private Seq<PackageReference> _packageReferences;
  private Seq<AssemblyReference> _assemblyReferences;
  private Seq<ProjectId> _referencedProjectIds;
  private string _rootNamespace = "";
  private HashMap<string, string> _properties;

  private CSharpProjectDtoBuilder(string assemblyName)
  {
    _assemblyName = assemblyName;
    _targetFramework = TargetFramework.Default;
    _projectId = new ProjectId(AbsolutePathTo(assemblyName).ToString());
  }

  public void WithReferences(params string[] names)
  {
    _referencedProjectIds = _referencedProjectIds.Concat(names.Select(n => new ProjectId(AbsolutePathTo(n).ToString())));
  }

  public void WithPackages(params string[] packageNames)
  {
    _packageReferences = _packageReferences.Concat(packageNames.Select(pn => new PackageReference(pn, "1.0.0")));
  }

  public void WithAssemblyReferences(params string[] assemblyNames)
  {
    _assemblyReferences = _assemblyReferences.Concat(assemblyNames.Select(an => new AssemblyReference(an, Any.String())));
  }

  public CSharpProjectDtoBuilder With(SourceCodeFileDtoBuilder sourceCodeFileDtoBuilder)
  {
    _sourceCodeFileBuilders = _sourceCodeFileBuilders.Add(sourceCodeFileDtoBuilder);
    return this;
  }

  public CSharpProjectDtoBuilder WithRootNamespace(string rootNamespace)
  {
    _rootNamespace = rootNamespace;
    return this;
  }

  public CSharpProjectDtoBuilder WithTargetFramework(string targetFramework)
  {
    _targetFramework = targetFramework;
    _properties = _properties.Add("TargetFramework", _targetFramework);
    return this;
  }

  private static AbsoluteFilePath AbsolutePathTo(string assemblyName)
  {
    return AbsoluteFilePath(
      FileSystemRoot.PlatformSpecificValue() + 
      Path.DirectorySeparatorChar + 
      assemblyName + 
      ".cs");
  }

  public static CSharpProjectDtoBuilder WithAssemblyName(string assemblyName)
  {
    return new CSharpProjectDtoBuilder(assemblyName);
  }

  public CsharpProjectDto BuildCsharpProjectDto()
  {
    return new CsharpProjectDto(
      _projectId,
      _assemblyName,
      _sourceCodeFileBuilders
        .Select(
          b => b.BuildWith(_assemblyName, _rootNamespace)).ToSeq(), 
      _properties, 
      _packageReferences.ToSeq(), 
      _assemblyReferences.ToSeq(), 
      _referencedProjectIds.ToSeq(),
      Seq.create(_targetFramework));
  }
}
