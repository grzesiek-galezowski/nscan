using GlobExpressions;
using NScan.Lib;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;
using NullableReferenceTypesExtensions;

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
    IFullProjectScopedRuleConstructed HasProperty(string propertyName, string propertyValue);
  }

  public interface IRuleDefinitionStart
  {
    IProjectNameStated Project(string dependingAssemblyName);
  }

  public interface IFullDependencyPathRuleConstructed
  {
    //bug replace with polymorphism!
    RuleUnionDto Build();

    DependencyPathBasedRuleUnionDto BuildDependencyPathBasedRule();
  }

  public interface IFullProjectScopedRuleConstructed
  {
    //bug replace with polymorphism!
    RuleUnionDto Build();

    ProjectScopedRuleUnionDto BuildProjectScopedRule();
  }
  
  public interface IFullNamespaceBasedRuleConstructed
  {
    //bug replace with polymorphism!
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
    private string? _exclusionPattern;
    private RuleUnionDto? _ruleUnionDto;
    private DependencyPathBasedRuleUnionDto? _dependencyPathRuleDto;
    private ProjectScopedRuleUnionDto? _projectScopedRuleDto;
    private NamespaceBasedRuleUnionDto? _namespaceBasedRuleDto;

    public IProjectNameStated Project(string dependingAssemblyName)
    {

      _dependingPattern = dependingAssemblyName;
      return this;
    }

    public IFullDependencyPathRuleConstructed IndependentOfProject(string dependentAssemblyName)
    {
      var complementDto = IndependentRuleComplement("project", dependentAssemblyName);
      _ruleUnionDto = RuleUnionDto.With(complementDto);
      _dependencyPathRuleDto =
        DependencyPathBasedRuleUnionDto.With(complementDto);
      return this;
    }

    public IFullDependencyPathRuleConstructed IndependentOfPackage(string packageName)
    {
      var complementDto = IndependentRuleComplement("package", packageName);
      _ruleUnionDto = RuleUnionDto.With(complementDto);
      _dependencyPathRuleDto =
        DependencyPathBasedRuleUnionDto.With(complementDto);
      return this;

    }

    public IFullDependencyPathRuleConstructed IndependentOfAssembly(string assemblyName)
    {
      var complementDto = IndependentRuleComplement("assembly", assemblyName);
      _ruleUnionDto = RuleUnionDto.With(complementDto);
      _dependencyPathRuleDto =
        DependencyPathBasedRuleUnionDto.With(complementDto);
      return this;
    }

    public IFullDependingPartStated Except(string exclusionPattern)
    {
      _exclusionPattern = exclusionPattern;
      return this;
    }

    public IFullProjectScopedRuleConstructed HasCorrectNamespaces()
    {
      var complementDto = CorrectNamespacesRuleComplement();
      _ruleUnionDto = RuleUnionDto.With(complementDto);
      _projectScopedRuleDto = ProjectScopedRuleUnionDto.With(complementDto);
      return this;
    }

    public IFullNamespaceBasedRuleConstructed HasNoCircularUsings()
    {
      var complementDto = NoCircularUsingsRuleComplement();
      _ruleUnionDto = RuleUnionDto.With(complementDto);
      _namespaceBasedRuleDto = NamespaceBasedRuleUnionDto.With(complementDto);
      return this;
    }

    public IFullProjectScopedRuleConstructed HasDecoratedMethods(string classInclusionPattern, string methodInclusionPattern)
    {
      var complementDto = HasAttributesOnRuleComplement(
        classInclusionPattern, 
        methodInclusionPattern);
      _ruleUnionDto = RuleUnionDto.With(complementDto);
      _projectScopedRuleDto = ProjectScopedRuleUnionDto.With(complementDto);
      return this;
    }

    public IFullProjectScopedRuleConstructed HasTargetFramework(string targetFramework)
    {
      var complementDto = HasTargetFrameworkRuleComplement(targetFramework);
      _ruleUnionDto = RuleUnionDto.With(complementDto);
      _projectScopedRuleDto = ProjectScopedRuleUnionDto.With(complementDto);
      return this;
    }

    public IFullNamespaceBasedRuleConstructed HasNoUsings(string @from, string to)
    {
      var complementDto = NoUsingsComplement(from, to);
      _ruleUnionDto = RuleUnionDto.With(complementDto);
      _namespaceBasedRuleDto = NamespaceBasedRuleUnionDto.With(complementDto);
      return this;
    }

    public IFullProjectScopedRuleConstructed HasProperty(string propertyName, string propertyValue)
    {
      var complementDto = HasPropertyComplement(propertyName, propertyValue);
      _ruleUnionDto = RuleUnionDto.With(complementDto);
      _projectScopedRuleDto = ProjectScopedRuleUnionDto.With(complementDto);
      return this;
    }

    //bug remove this  later
    public RuleUnionDto Build()
    {
      return _ruleUnionDto.OrThrow();
    }

    private HasPropertyRuleComplementDto HasPropertyComplement(string propertyName, string propertyValue)
    {
      return new HasPropertyRuleComplementDto(
        GetDependingPattern(), 
        propertyName, 
        propertyValue);
    }

    public DependencyPathBasedRuleUnionDto BuildDependencyPathBasedRule()
    {
      return _dependencyPathRuleDto.OrThrow();
    }

    public ProjectScopedRuleUnionDto BuildProjectScopedRule()
    {
      return _projectScopedRuleDto.OrThrow();
    }

    public NamespaceBasedRuleUnionDto BuildNamespaceBasedRule()
    {
      return _namespaceBasedRuleDto.OrThrow();
    }

    private HasTargetFrameworkRuleComplementDto HasTargetFrameworkRuleComplement(string targetFramework)
    {
      return new HasTargetFrameworkRuleComplementDto(
        GetDependingPattern(), 
        targetFramework);
    }

    private NoUsingsRuleComplementDto NoUsingsComplement(string @from, string to)
    {
      return new(
        GetDependingPattern(),
        Pattern.WithoutExclusion(@from),
        Pattern.WithoutExclusion(to)
      );
    }
    
    private NoCircularUsingsRuleComplementDto NoCircularUsingsRuleComplement()
    {
      return new(GetDependingPattern());
    }
    
    private HasAttributesOnRuleComplementDto HasAttributesOnRuleComplement(string classInclusionPattern, string methodInclusionPattern)
    {
      return new(
        GetDependingPattern(),
        Pattern.WithoutExclusion(classInclusionPattern),
        Pattern.WithoutExclusion(methodInclusionPattern)
      );
    }
    
    private CorrectNamespacesRuleComplementDto CorrectNamespacesRuleComplement()
    {
      return new(GetDependingPattern());
    }

    private IndependentRuleComplementDto IndependentRuleComplement(string dependencyType, string dependencyName)
    {
      return new(
        dependencyType, 
        GetDependingPattern(),
        new Glob(dependencyName));
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
