using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Domain.Root
{
  public class RuleFactory : IRuleFactory, IDependencyBasedRuleFactory, IProjectScopedRuleFactory, INamespaceBasedRuleFactory
  {
    private readonly IRuleViolationFactory _ruleViolationFactory;
    private readonly IDependencyBasedRuleFactory _dependencyPathRuleFactory;

    public RuleFactory(IRuleViolationFactory ruleViolationFactory)
    {
      _ruleViolationFactory = ruleViolationFactory;
      _dependencyPathRuleFactory = new DependencyPathRuleFactory(ruleViolationFactory);
    }

    public IDependencyRule CreateDependencyRuleFrom(IndependentRuleComplementDto independentRuleComplementDto)
    {
      return _dependencyPathRuleFactory.CreateDependencyRuleFrom(independentRuleComplementDto);
    }

    public IProjectScopedRule CreateProjectScopedRuleFrom(CorrectNamespacesRuleComplementDto ruleDto)
    {
      return new ProjectScopedRuleApplicableToMatchingProject(ruleDto.ProjectAssemblyNamePattern, 
        new ProjectSourceCodeFilesRelatedRule(HasCorrectNamespacesRuleMetadata.Format(ruleDto), 
        new CorrectNamespacesInFileCheck()));
    }

    public IProjectScopedRule CreateProjectScopedRuleFrom(HasAttributesOnRuleComplementDto ruleDto)
    {
      return new ProjectScopedRuleApplicableToMatchingProject(
        ruleDto.ProjectAssemblyNamePattern,
        new ProjectSourceCodeFilesRelatedRule(HasAttributesOnRuleMetadata.Format(ruleDto),
          new MethodsOfMatchingClassesAreDecoratedWithAttributeCheck(ruleDto)));
    }

    public IProjectScopedRule CreateProjectScopedRuleFrom(HasTargetFrameworkRuleComplementDto ruleDto)
    {
      return 
        new ProjectScopedRuleApplicableToMatchingProject(
          ruleDto.ProjectAssemblyNamePattern,
          new HasTargetFrameworkRule(ruleDto.TargetFramework, 
            _ruleViolationFactory, 
            HasTargetFrameworkRuleMetadata.Format(ruleDto)));
    }

    public INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoCircularUsingsRuleComplementDto ruleDto)
    {
      return new NoCircularUsingsRule(ruleDto, _ruleViolationFactory);
    }

    public INamespacesBasedRule CreateNamespacesBasedRuleFrom(NoUsingsRuleComplementDto ruleDto)
    {
      return new NoUsingsRule(ruleDto);
    }
  }


}
