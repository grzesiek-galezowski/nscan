using NScan.ProjectScopedRules;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Domain.Root
{
  //bug move to specific project
  public class ProjectScopedRuleFactory : IProjectScopedRuleFactory
  {
    private readonly IProjectScopedRuleViolationFactory _ruleViolationFactory;

    public ProjectScopedRuleFactory(IProjectScopedRuleViolationFactory ruleViolationFactory)
    {
      _ruleViolationFactory = ruleViolationFactory;
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
          new HasTargetFrameworkRule(ruleDto.TargetFramework, _ruleViolationFactory, 
            HasTargetFrameworkRuleMetadata.Format(ruleDto)));
    }
  }
}