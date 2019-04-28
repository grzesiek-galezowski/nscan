using System;
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
    IFullRuleConstructed ToHaveDecoratedMethods(string classInclusionPattern, string methodInclusionPattern);
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
    private string? _dependingPattern;
    private string? _ruleName;
    private Maybe<string> _exclusionPattern = Maybe<string>.Nothing;
    private string? _dependencyName;
    private string? _dependencyType;
    private string? _classInclusionPattern;
    private string? _methodInclusionPattern;

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

    public IFullRuleConstructed ToHaveDecoratedMethods(string classInclusionPattern, string methodInclusionPattern)
    {
      _ruleName = RuleNames.HasAttributesOn;
      _classInclusionPattern = classInclusionPattern;
      _methodInclusionPattern = methodInclusionPattern;
      return this;
    }

    public RuleUnionDto Build()
    {
      var dependingPattern = _exclusionPattern
        .Select(p => Pattern.WithExclusion(_dependingPattern!.OrThrow(), p))
        .OrElse(() => Pattern.WithoutExclusion(_dependingPattern!.OrThrow()));

      return RuleNames.Switch(
        _ruleName!.OrThrow(),
        () => RuleUnionDto.With(new IndependentRuleComplementDto(
          _dependencyType!.OrThrow(), 
          dependingPattern,
          new Glob(_dependencyName))),
        () => RuleUnionDto.With(new CorrectNamespacesRuleComplementDto(dependingPattern)),
        () => RuleUnionDto.With(new NoCircularUsingsRuleComplementDto(dependingPattern)),
        () => RuleUnionDto.With(new IsAnnotatedRuleComplementDto(
            dependingPattern,
            Pattern.WithoutExclusion(_classInclusionPattern ??
                                     throw new ArgumentNullException(nameof(_classInclusionPattern))),
            Pattern.WithoutExclusion(_methodInclusionPattern ??
                                     throw new ArgumentNullException(nameof(_methodInclusionPattern)))
          )
        ));
    }


    public static IRuleDefinitionStart RuleRequiring()
    {
      return new DependencyRuleBuilder();
    }
  }

  public static class X
  {
    public static T OrThrow<T>(this T instance) where T : class
    {
      return instance ?? throw new ArgumentNullException(nameof(instance));
    }
  }
}