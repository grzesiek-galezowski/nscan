using GlobExpressions;
using NScan.DependencyPathBasedRules;
using NScan.Lib;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Domain.Root
{
  public class RuleFactory : IRuleFactory
  {
    public const string ProjectDependencyType = "project";
    public const string PackageDependencyType = "package";
    public const string AssemblyDependencyType = "assembly";


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

    public IProjectScopedRule CreateProjectScopedRuleFrom(CorrectNamespacesRuleComplementDto ruleDto)
    {
      return new ProjectScopedRuleApplicableToMatchingProject(ruleDto.ProjectAssemblyNamePattern, 
        new ProjectSourceCodeFilesRelatedRule(RuleDescription(ruleDto), 
        new CorrectNamespacesInFileCheck()));
    }

    public IProjectScopedRule CreateProjectScopedRuleFrom(HasAttributesOnRuleComplementDto ruleDto)
    {
      return new ProjectScopedRuleApplicableToMatchingProject(
        ruleDto.ProjectAssemblyNamePattern,
        new ProjectSourceCodeFilesRelatedRule($"{ruleDto.ProjectAssemblyNamePattern.Description()} {ruleDto.RuleName} " +
          $"{ruleDto.ClassNameInclusionPattern.Description()}:{ruleDto.MethodNameInclusionPattern.Description()}",
          new MethodsOfMatchingClassesAreDecoratedWithAttributeCheck(ruleDto)));
    }

    public IProjectScopedRule CreateProjectScopedRuleFrom(HasTargetFrameworkRuleComplementDto ruleDto)
    {
      return 
        new ProjectScopedRuleApplicableToMatchingProject(
          ruleDto.ProjectAssemblyNamePattern,
          new HasTargetFrameworkRule(ruleDto.TargetFramework, 
            RuleViolationFactory(), 
            RuleDescription(ruleDto)));
    }

    private string RuleDescription(HasTargetFrameworkRuleComplementDto ruleDto)
    {
      return ruleDto.ProjectAssemblyNamePattern.Description() + " " + ruleDto.RuleName;
    }

    private static string RuleDescription(CorrectNamespacesRuleComplementDto ruleDto)
    {
      return ruleDto.ProjectAssemblyNamePattern.Description() + " " + ruleDto.RuleName;
    }

    public INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoCircularUsingsRuleComplementDto ruleDto)
    {
      return new NoCircularUsingsRule(ruleDto, RuleViolationFactory());
    }

    public INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoUsingsRuleComplementDto ruleDto)
    {
      return new NoUsingsRule(ruleDto);
    }

    private static IRuleViolationFactory RuleViolationFactory()
    {
      return new RuleViolationFactory(new PlainReportFragmentsFormat());
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
        dependingNamePattern,
        RuleViolationFactory());
    }

    private static IDependencyRule CreateIndependentOfPackageRule(
      Pattern dependingAssemblyNamePattern,
      Glob packageNamePattern,
      string dependencyType)
    {
      return new IndependentRule(
        new DescribedCondition(
          new HasPackageReferenceMatchingCondition(packageNamePattern), 
          IndependentRuleMetadata.FormatIndependentRule(dependingAssemblyNamePattern, dependencyType, packageNamePattern)), 
        dependingAssemblyNamePattern, RuleViolationFactory());
    }

    private static IDependencyRule CreateIndependentOfAssemblyRule(
      Pattern dependingAssemblyNamePattern,
      Glob assemblyNamePattern,
      string dependencyType)
    {
      return new IndependentRule(new DescribedCondition(new HasAssemblyReferenceMatchingCondition(assemblyNamePattern), IndependentRuleMetadata.FormatIndependentRule(dependingAssemblyNamePattern, dependencyType, assemblyNamePattern)), 
        dependingAssemblyNamePattern, RuleViolationFactory());
    }
  }
}
