using Functional.Maybe;
using Functional.Maybe.Just;
using GlobExpressions;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Specification.Component.AutomationLayer
{
  public interface IFullDependingPartStated
  {
    IFullRuleConstructed IndependentOfProject(string dependentAssemblyName);
    IFullRuleConstructed IndependentOfPackage(string packageName);
    IFullRuleConstructed IndependentOfAssembly(string assemblyName);
  }

  public interface IProjectNameStated : IFullDependingPartStated
  {
    IFullDependingPartStated Except(string exclusionPattern);
    IFullRuleConstructed HasCorrectNamespaces();
    IFullRuleConstructed HasNoCircularUsings();
  }

  public interface IRuleDefinitionStart
  {
    IProjectNameStated Project(string dependingAssemblyName);
  }

  public interface IFullRuleConstructed
  {
    RuleUnionDto Build();
  }

  public class DependencyRuleBuilder : IRuleDefinitionStart, IFullRuleConstructed, IProjectNameStated
  {
    private string _dependingPattern;
    private string _ruleName;
    private Maybe<string> _exclusionPattern = Maybe<string>.Nothing;
    private string _dependencyName;
    private string _dependencyType;

    public IProjectNameStated Project(string dependingAssemblyName)
    {

      _dependingPattern = dependingAssemblyName;
      return this;
    }

    public IFullRuleConstructed IndependentOfProject(string dependentAssemblyName)
    {
      _dependencyName = dependentAssemblyName;
      _dependencyType = "project";
      _ruleName = RuleNames.IndependentOf;
      return this;
    }

    public IFullRuleConstructed IndependentOfPackage(string packageName)
    {
      _dependencyName = packageName;
      _dependencyType = "package";
      _ruleName = RuleNames.IndependentOf;
      return this;

    }

    public IFullRuleConstructed IndependentOfAssembly(string assemblyName)
    {
      _dependencyName = assemblyName;
      _ruleName = RuleNames.IndependentOf;
      _dependencyType = "assembly";
      return this;
    }

    public IFullDependingPartStated Except(string exclusionPattern)
    {
      _exclusionPattern = exclusionPattern.Just();
      return this;
    }

    public IFullRuleConstructed HasCorrectNamespaces()
    {
      _ruleName = RuleNames.HasCorrectNamespaces;
      return this;
    }

    public IFullRuleConstructed HasNoCircularUsings()
    {
      _ruleName = RuleNames.HasNoCircularUsings;
      return this;
    }

    public RuleUnionDto Build()
    {
      var dependingPattern = _exclusionPattern
        .Select(p => Pattern.WithExclusion(_dependingPattern, p))
        .OrElse(() => Pattern.WithoutExclusion(_dependingPattern));

      return RuleNames.Switch(
        _ruleName,
        () => RuleUnionDto.With(new IndependentRuleComplementDto
        {
          DependencyType = _dependencyType,
          DependencyPattern = new Glob(_dependencyName),
          DependingPattern = dependingPattern
        }),
        () => RuleUnionDto.With(new CorrectNamespacesRuleComplementDto
        {
          ProjectAssemblyNamePattern = dependingPattern
        }), 
        () => RuleUnionDto.With(new NoCircularUsingsRuleComplementDto
        {
          ProjectAssemblyNamePattern = dependingPattern
        }));
    }


    public static IRuleDefinitionStart RuleRequiring()
    {
      return new DependencyRuleBuilder();
    }
  }

}