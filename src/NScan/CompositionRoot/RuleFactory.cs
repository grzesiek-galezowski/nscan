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
      var dependingAssemblyNamePattern = ruleDto.DependingPattern;
      if (ruleDto.DependencyType == ProjectDependencyType)
      {
        rule = CreateIndependentOfProjectRule(dependingAssemblyNamePattern, ruleDto.DependencyPattern, ruleDto.DependencyType);
      }
      else if (ruleDto.DependencyType == PackageDependencyType)
      {
        rule = CreateIndependentOfPackageRule(dependingAssemblyNamePattern, ruleDto.DependencyPattern, ruleDto.DependencyType);
      }
      else if (ruleDto.DependencyType == AssemblyDependencyType)
      {
        rule = CreateIndependentOfAssemblyRule(dependingAssemblyNamePattern, ruleDto.DependencyPattern, ruleDto.DependencyType);
      }
      else
      {
        throw new InvalidRuleException(ruleDto.DependencyType);
      }

      return rule;
    }

    private IDependencyRule CreateIndependentOfProjectRule(Pattern dependingNamePattern,
      Glob dependencyNamePattern,
      string dependencyType)
    {
      return new IndependentRule(
        new JoinedDescribedCondition(new IsFollowingAssemblyCondition(),
          new HasAssemblyNameMatchingPatternCondition(
            dependencyNamePattern),
          Description(dependingNamePattern, 
            dependencyNamePattern, 
            dependencyType)), 
        dependingNamePattern);
    }

    private static IDependencyRule CreateIndependentOfPackageRule(
      Pattern dependingAssemblyNamePattern,
      Glob packageNamePattern,
      string dependencyType)
    {
      return new IndependentRule(
        new DescribedCondition(
          new HasPackageReferenceMatchingCondition(packageNamePattern),
          Description(dependingAssemblyNamePattern, 
            packageNamePattern, dependencyType)), 
        dependingAssemblyNamePattern);
    }

    private static IDependencyRule CreateIndependentOfAssemblyRule(
      Pattern dependingAssemblyNamePattern,
      Glob assemblyNamePattern,
      string dependencyType)
    {
      return new IndependentRule(new DescribedCondition(new HasAssemblyReferenceMatchingCondition(assemblyNamePattern),
        Description(dependingAssemblyNamePattern, 
          assemblyNamePattern, dependencyType)), 
        dependingAssemblyNamePattern);
    }

    private static string Description(Pattern dependingNamePattern, Glob assemblyNamePattern,
      string dependencyType)
    {
      //TODO consider moving to DependencyDescriptions
      return DependencyDescriptions.IndependentOf(dependingNamePattern.Description(),
        dependencyType + ":" + assemblyNamePattern.Pattern);
    }
  }
}