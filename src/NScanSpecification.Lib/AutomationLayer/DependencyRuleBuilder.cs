using System;
using GlobExpressions;
using NScan.Lib;
using NScan.SharedKernel.Ports;
using NScan.SharedKernel.SharedKernel;

namespace NScanSpecification.Lib.AutomationLayer
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
    IFullRuleConstructed HasTargetFramework(string targetFramework);
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
    private string? _exclusionPattern;
    private string? _dependencyName;
    private string? _dependencyType;
    private string? _classInclusionPattern;
    private string? _methodInclusionPattern;
    private string? _targetFramework;

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
      _exclusionPattern = exclusionPattern;
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

    public IFullRuleConstructed HasTargetFramework(string targetFramework)
    {
      _ruleName = RuleNames.HasTargetFramework;
      _targetFramework = targetFramework;
      return this;
    }

    public RuleUnionDto Build()
    {
      var dependingPattern = _exclusionPattern
        .Select(p => Pattern.WithExclusion(_dependingPattern.OrThrow(), p))
        .OrElse(() => Pattern.WithoutExclusion(_dependingPattern.OrThrow()));

      return RuleNames.Switch(
        _ruleName.OrThrow(),
        IndependentOf(dependingPattern),
        CorrectNamespaces(dependingPattern),
        NoCircularUsings(dependingPattern),
        HasAttributesOnMethods(dependingPattern),
        () => RuleUnionDto.With(new HasTargetFrameworkRuleComplementDto(dependingPattern, 
          _targetFramework.OrThrow())));
    }

    private Func<RuleUnionDto> HasAttributesOnMethods(Pattern dependingPattern)
    {
      return () => RuleUnionDto.With(new HasAttributesOnRuleComplementDto(
        dependingPattern,
        Pattern.WithoutExclusion(_classInclusionPattern.OrThrow(nameof(_classInclusionPattern))),
        Pattern.WithoutExclusion(_methodInclusionPattern.OrThrow(nameof(_methodInclusionPattern)))
      ));
    }

    private static Func<RuleUnionDto> NoCircularUsings(Pattern dependingPattern)
    {
      return () => RuleUnionDto.With(new NoCircularUsingsRuleComplementDto(dependingPattern));
    }

    private static Func<RuleUnionDto> CorrectNamespaces(Pattern dependingPattern)
    {
      return () => RuleUnionDto.With(new CorrectNamespacesRuleComplementDto(dependingPattern));
    }

    private Func<RuleUnionDto> IndependentOf(Pattern dependingPattern)
    {
      return () => RuleUnionDto.With(new IndependentRuleComplementDto(
        _dependencyType.OrThrow(), 
        dependingPattern,
        new Glob(_dependencyName)));
    }


    public static IRuleDefinitionStart RuleDemandingThat()
    {
      return new DependencyRuleBuilder();
    }
  }

  public static class X
  {
    public static T OrThrow<T>(this T? instance) where T : class
    {
      return instance.OrThrow(nameof(instance));
    }

    public static T OrThrow<T>(this T? instance, string instanceName) where T : class
    {
      return instance ?? throw new ArgumentNullException(instanceName);
    }

    public static TResult? Select<T, TResult>(this T? a, Func<T, TResult> fn) 
      where T: class 
      where TResult : class
    {
      return a == null ? null : fn(a);
    }

    public static T OrElse<T>(this T? a, Func<T> @default) where T : class
    {
      return a ?? @default();
    }
  }
}