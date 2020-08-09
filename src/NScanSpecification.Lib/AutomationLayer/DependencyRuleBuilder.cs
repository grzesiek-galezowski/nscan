using GlobExpressions;
using NScan.Lib;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScanSpecification.Lib.AutomationLayer
{
  public interface IFullDependingPartStated
  {
    IFullDependencyPathRuleConstructed IndependentOfProject(string dependentAssemblyName);
    IFullDependencyPathRuleConstructed IndependentOfPackage(string packageName);
    IFullDependencyPathRuleConstructed IndependentOfAssembly(string assemblyName);
  }

  public interface IProjectNameStated : IFullDependingPartStated
  {
    IFullDependingPartStated Except(string exclusionPattern);
    IFullProjectScopedRuleConstructed HasCorrectNamespaces();
    IFullNamespaceBasedRuleConstructed HasNoCircularUsings();
    IFullProjectScopedRuleConstructed HasDecoratedMethods(string classInclusionPattern, string methodInclusionPattern);
    IFullProjectScopedRuleConstructed HasTargetFramework(string targetFramework);
    IFullNamespaceBasedRuleConstructed HasNoUsings(string from, string to);
  }

  public interface IRuleDefinitionStart
  {
    IProjectNameStated Project(string dependingAssemblyName);
  }

  public interface IFullDependencyPathRuleConstructed
  {
    RuleUnionDto Build();
    DependencyPathBasedRuleUnionDto BuildDependencyPathBasedRule();
  }

  public interface IFullProjectScopedRuleConstructed
  {
    RuleUnionDto Build();
    ProjectScopedRuleUnionDto BuildProjectScopedRule();
  }
  
  public interface IFullNamespaceBasedRuleConstructed
  {
    RuleUnionDto Build();
    NamespaceBasedRuleUnionDto BuildNamespaceBasedRule();
  }

  public class DependencyRuleBuilder : 
    IRuleDefinitionStart, 
    IFullDependencyPathRuleConstructed, 
    IFullProjectScopedRuleConstructed, 
    IFullNamespaceBasedRuleConstructed, 
    IProjectNameStated 
  {
    private string? _dependingPattern;
    private string? _ruleName;
    private string? _exclusionPattern;
    private string? _dependencyName;
    private string? _dependencyType;
    private string? _classInclusionPattern;
    private string? _methodInclusionPattern;
    private string? _targetFramework;
    private string? _from;
    private string? _to;

    public IProjectNameStated Project(string dependingAssemblyName)
    {

      _dependingPattern = dependingAssemblyName;
      return this;
    }

    public IFullDependencyPathRuleConstructed IndependentOfProject(string dependentAssemblyName)
    {
      _dependencyName = dependentAssemblyName;
      _dependencyType = "project";
      _ruleName = IndependentRuleMetadata.IndependentOf;
      return this;
    }

    public IFullDependencyPathRuleConstructed IndependentOfPackage(string packageName)
    {
      _dependencyName = packageName;
      _dependencyType = "package";
      _ruleName = IndependentRuleMetadata.IndependentOf;
      return this;

    }

    public IFullDependencyPathRuleConstructed IndependentOfAssembly(string assemblyName)
    {
      _dependencyName = assemblyName;
      _ruleName = IndependentRuleMetadata.IndependentOf;
      _dependencyType = "assembly";
      return this;
    }

    public IFullDependingPartStated Except(string exclusionPattern)
    {
      _exclusionPattern = exclusionPattern;
      return this;
    }

    public IFullProjectScopedRuleConstructed HasCorrectNamespaces()
    {
      _ruleName = HasCorrectNamespacesRuleMetadata.HasCorrectNamespaces;
      return this;
    }

    public IFullNamespaceBasedRuleConstructed HasNoCircularUsings()
    {
      _ruleName = HasNoCircularUsingsRuleMetadata.HasNoCircularUsings;
      return this;
    }

    public IFullProjectScopedRuleConstructed HasDecoratedMethods(string classInclusionPattern, string methodInclusionPattern)
    {
      _ruleName = HasAttributesOnRuleMetadata.HasAttributesOn;
      _classInclusionPattern = classInclusionPattern;
      _methodInclusionPattern = methodInclusionPattern;
      return this;
    }

    public IFullProjectScopedRuleConstructed HasTargetFramework(string targetFramework)
    {
      _ruleName = HasTargetFrameworkRuleMetadata.HasTargetFramework;
      _targetFramework = targetFramework;
      return this;
    }

    public IFullNamespaceBasedRuleConstructed HasNoUsings(string @from, string to)
    {
      _ruleName = "hasNoUsings";
      _from = from;
      _to = to;
      return this;
    }

    //bug remove later
    public RuleUnionDto Build()
    {
      return RuleNames.Switch(
        _ruleName.OrThrow(),
        IndependentOf,
        CorrectNamespaces,
        NoCircularUsings,
        NoUsings,
        HasAttributesOnMethods,
        HasTargetFramework);
    }

    private RuleUnionDto HasTargetFramework()
    {
      return RuleUnionDto.With(HasTargetFrameworkRuleComplement());
    }

    private RuleUnionDto NoUsings()
    {
      return RuleUnionDto.With(NoUsingsComplement());
    }

    public DependencyPathBasedRuleUnionDto BuildDependencyPathBasedRule()
    {
      return DependencyPathBasedRuleUnionDto.With(IndependentRuleComplement());
    }

    public ProjectScopedRuleUnionDto BuildProjectScopedRule()
    {
      return ProjectScopedRuleNames.Switch(
        _ruleName.OrThrow(),
        () => ProjectScopedRuleUnionDto.With(CorrectNamespacesRuleComplement()),
        () => ProjectScopedRuleUnionDto.With(HasAttributesOnRuleComplement()),
        () => ProjectScopedRuleUnionDto.With(HasTargetFrameworkRuleComplement())
        );
    }

    public NamespaceBasedRuleUnionDto BuildNamespaceBasedRule()
    {
      return NamespaceBasedRuleNames.Switch(_ruleName.OrThrow(),
        () => NamespaceBasedRuleUnionDto.With(NoCircularUsingsRuleComplement()),
        () => NamespaceBasedRuleUnionDto.With(NoUsingsComplement()));
    }

    private RuleUnionDto HasAttributesOnMethods()
    {
      return RuleUnionDto.With(HasAttributesOnRuleComplement());
    }

    private RuleUnionDto NoCircularUsings()
    {
      return RuleUnionDto.With(NoCircularUsingsRuleComplement());
    }

    private RuleUnionDto CorrectNamespaces()
    {
      return RuleUnionDto.With(CorrectNamespacesRuleComplement());
    }
    private RuleUnionDto IndependentOf()
    {
      return RuleUnionDto.With(IndependentRuleComplement());
    }

    private HasTargetFrameworkRuleComplementDto HasTargetFrameworkRuleComplement()
    {
      return new HasTargetFrameworkRuleComplementDto(GetDependingPattern(), 
        _targetFramework.OrThrow());
    }

    private NoUsingsRuleComplementDto NoUsingsComplement()
    {
      return new NoUsingsRuleComplementDto(
        GetDependingPattern(),
        Pattern.WithoutExclusion(_from.OrThrow(nameof(_from))),
        Pattern.WithoutExclusion(_to.OrThrow(nameof(_to)))
      );
    }
    
    private NoCircularUsingsRuleComplementDto NoCircularUsingsRuleComplement()
    {
      return new NoCircularUsingsRuleComplementDto(GetDependingPattern());
    }
    
    private HasAttributesOnRuleComplementDto HasAttributesOnRuleComplement()
    {
      return new HasAttributesOnRuleComplementDto(
        GetDependingPattern(),
        Pattern.WithoutExclusion(_classInclusionPattern.OrThrow(nameof(_classInclusionPattern))),
        Pattern.WithoutExclusion(_methodInclusionPattern.OrThrow(nameof(_methodInclusionPattern)))
      );
    }
    
    private CorrectNamespacesRuleComplementDto CorrectNamespacesRuleComplement()
    {
      return new CorrectNamespacesRuleComplementDto(GetDependingPattern());
    }

    private IndependentRuleComplementDto IndependentRuleComplement()
    {
      return new IndependentRuleComplementDto(
        _dependencyType.OrThrow(), 
        GetDependingPattern(),
        new Glob(_dependencyName));
    }

    public static IRuleDefinitionStart RuleDemandingThat()
    {
      return new DependencyRuleBuilder();
    }

    private Pattern GetDependingPattern()
    {
      return _exclusionPattern
        .Select(p => Pattern.WithExclusion(_dependingPattern.OrThrow(), p))
        .OrElse(() => Pattern.WithoutExclusion(_dependingPattern.OrThrow()));
    }
  }
}