using GlobExpressions;
using NScan.DependencyPathBasedRules;
using NScan.Lib;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;

namespace NScan.Domain.Root
{
  public class DependencyPathRuleFactory : IDependencyBasedRuleFactory
  {
    public const string ProjectDependencyType = "project";
    public const string PackageDependencyType = "package";
    public const string AssemblyDependencyType = "assembly";
    private readonly IDependencyPathRuleViolationFactory _ruleViolationFactory;

    public DependencyPathRuleFactory(IDependencyPathRuleViolationFactory ruleViolationFactory)
    {
      _ruleViolationFactory = ruleViolationFactory;
    }

    public IDependencyRule CreateDependencyRuleFrom(IndependentRuleComplementDto independentRuleComplementDto)
    {
      var dependingAssemblyNamePattern = independentRuleComplementDto.DependingPattern;
      if (independentRuleComplementDto.DependencyType == ProjectDependencyType)
      {
        return CreateIndependentOfProjectRule(
          dependingAssemblyNamePattern, 
          independentRuleComplementDto.DependencyPattern, 
          independentRuleComplementDto.DependencyType);
      }
      else if (independentRuleComplementDto.DependencyType == PackageDependencyType)
      {
        return CreateIndependentOfPackageRule(
          dependingAssemblyNamePattern, 
          independentRuleComplementDto.DependencyPattern, 
          independentRuleComplementDto.DependencyType);
      }
      else if (independentRuleComplementDto.DependencyType == AssemblyDependencyType)
      {
        return CreateIndependentOfAssemblyRule(
          dependingAssemblyNamePattern, 
          independentRuleComplementDto.DependencyPattern, 
          independentRuleComplementDto.DependencyType);
      }
      else
      {
        throw new InvalidRuleException(independentRuleComplementDto.DependencyType);
      }
    }

    private IDependencyRule CreateIndependentOfProjectRule(Pattern dependingNamePattern,
      Glob dependencyNamePattern,
      string dependencyType)
    {
      return new IndependentRule(
        new JoinedDescribedCondition(new IsFollowingAssemblyCondition(),
          new HasAssemblyNameMatchingPatternCondition(
            dependencyNamePattern), IndependentRuleMetadata.FormatIndependentRule(dependingNamePattern, 
            dependencyType, dependencyNamePattern)), 
        dependingNamePattern, _ruleViolationFactory);
    }

    private IDependencyRule CreateIndependentOfPackageRule(
      Pattern dependingAssemblyNamePattern,
      Glob packageNamePattern,
      string dependencyType)
    {
      return new IndependentRule(
        new DescribedCondition(
          new HasPackageReferenceMatchingCondition(packageNamePattern), 
          IndependentRuleMetadata.FormatIndependentRule(dependingAssemblyNamePattern, dependencyType, packageNamePattern)), 
        dependingAssemblyNamePattern, _ruleViolationFactory);
    }

    private IDependencyRule CreateIndependentOfAssemblyRule(
      Pattern dependingAssemblyNamePattern,
      Glob assemblyNamePattern,
      string dependencyType)
    {
      return new IndependentRule(new DescribedCondition(new HasAssemblyReferenceMatchingCondition(assemblyNamePattern), IndependentRuleMetadata.FormatIndependentRule(dependingAssemblyNamePattern, dependencyType, assemblyNamePattern)), 
        dependingAssemblyNamePattern, _ruleViolationFactory);
    }
  }
}