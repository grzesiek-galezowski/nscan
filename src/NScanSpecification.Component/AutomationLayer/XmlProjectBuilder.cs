using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using AtmaFileSystem;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScanSpecification.Lib.AutomationLayer;
using TddXt.AnyRoot.Strings;
using static AtmaFileSystem.AtmaFileSystemPaths;
using static TddXt.AnyRoot.Root;

namespace NScanSpecification.Component.AutomationLayer
{
  public class XmlProjectBuilder
  {
    private readonly string _assemblyName;
    private string _targetFramework;
    private readonly List<SourceCodeFileDtoBuilder> _sourceCodeFileBuilders = new();
    private readonly ProjectId _projectId;
    private readonly List<PackageReference> _packageReferences = new();
    private readonly List<AssemblyReference> _assemblyReferences = new();
    private readonly List<ProjectId> _referencedProjectIds = new();
    private string _rootNamespace = "";

    private XmlProjectBuilder(string assemblyName)
    {
      _assemblyName = assemblyName;
      _targetFramework = "netcore21";
      _projectId = new ProjectId(AbsolutePathTo(assemblyName).ToString());
    }

    public void WithReferences(params string[] names)
    {
      _referencedProjectIds.AddRange(names.Select(n => new ProjectId(AbsolutePathTo(n).ToString())));
    }

    public void WithPackages(params string[] packageNames)
    {
      _packageReferences.AddRange(packageNames.Select(pn => new PackageReference(pn, "1.0.0")));
    }

    public void WithAssemblyReferences(params string[] assemblyNames)
    {
      _assemblyReferences.AddRange(assemblyNames.Select(an => new AssemblyReference(an, Any.String())));
    }

    public XmlProjectBuilder With(SourceCodeFileDtoBuilder sourceCodeFileDtoBuilder)
    {
      _sourceCodeFileBuilders.Add(sourceCodeFileDtoBuilder);
      return this;
    }

    public XmlProjectBuilder WithRootNamespace(string rootNamespace)
    {
      _rootNamespace = rootNamespace;
      return this;
    }

    public XmlProjectBuilder WithTargetFramework(string targetFramework)
    {
      _targetFramework = targetFramework;
      return this;
    }

    private static AbsoluteFilePath AbsolutePathTo(string assemblyName)
    {
      return AbsoluteFilePath(@"C:\" + assemblyName + ".cs");
    }

    public static XmlProjectBuilder WithAssemblyName(string assemblyName)
    {
      return new(assemblyName);
    }

    public CsharpProjectDto BuildCsharpProjectDto()
    {
      return new(_projectId,
        _assemblyName,
        _targetFramework,
        _sourceCodeFileBuilders
          .Select(
            b => b.BuildWith(_assemblyName, _rootNamespace)).ToImmutableList(), 
        _packageReferences.ToImmutableList(), 
        _assemblyReferences.ToImmutableList(), 
        _referencedProjectIds.ToImmutableList());
    }
  }
}
