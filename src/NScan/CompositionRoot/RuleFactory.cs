
using System.Data;
using GlobExpressions;

namespace TddXt.NScan.CompositionRoot
{
  public class RuleFactory : IRuleFactory
  {
    public const string ProjectDependencyType = "project";
    public const string PackageDependencyType = "package";
    public const string AssemblyDependencyType = "assembly";


    public IDependencyRule CreateDependencyRuleFrom(RuleDto ruleDto)
    {
      IDependencyRule rule = null;
      if (ruleDto.DependencyType == ProjectDependencyType)
      {
        rule = CreateIndependentOfProjectRule(ruleDto.DependingPattern, ruleDto.DependencyPattern, ruleDto.DependencyType);
      }
      else if (ruleDto.DependencyType == PackageDependencyType)
      {
        rule = CreateIndependentOfPackageRule(ruleDto.DependingPattern, ruleDto.DependencyPattern, ruleDto.DependencyType);
      }
      else if (ruleDto.DependencyType == AssemblyDependencyType)
      {
        rule = CreateIndependentOfAssemblyRule(ruleDto.DependingPattern, ruleDto.DependencyPattern, ruleDto.DependencyType);
      }
      else
      {
        throw new InvalidRuleException(ruleDto.DependencyType);
      }

      return rule;
    }

    private IDependencyRule CreateIndependentOfProjectRule(Glob dependingNamePattern, Glob dependencyNamePattern,
      string dependencyType)
    {
      return new IndependentRule(
        new JoinedDescribedCondition(new IsFollowingAssemblyCondition(),
          new HasAssemblyNameMatchingPatternCondition(
            dependencyNamePattern),
          Description(dependingNamePattern, dependencyNamePattern, dependencyType)),
        dependingNamePattern);
    }

    private IDependencyRule CreateIndependentOfPackageRule(Glob dependingNamePattern, Glob packageNamePattern,
      string dependencyType)
    {
      return new IndependentRule(
        new DescribedCondition(
          new HasPackageReferenceMatchingCondition(packageNamePattern),
          Description(dependingNamePattern, packageNamePattern, dependencyType)), dependingNamePattern);
    }

    private IDependencyRule CreateIndependentOfAssemblyRule(Glob dependingNamePattern, Glob assemblyNamePattern,
      string dependencyType)
    {
      return new IndependentRule(new DescribedCondition(new HasAssemblyReferenceMatchingCondition(assemblyNamePattern),
        Description(dependingNamePattern, assemblyNamePattern, dependencyType)), dependingNamePattern);
    }

    private static string Description(Glob dependingNamePattern, Glob assemblyNamePattern, string dependencyType)
    {
      //TODO consider moving to DependencyDescriptions
      return DependencyDescriptions.IndependentOf(dependingNamePattern.Pattern,
        dependencyType + ":" + assemblyNamePattern.Pattern);
    }
  }
}