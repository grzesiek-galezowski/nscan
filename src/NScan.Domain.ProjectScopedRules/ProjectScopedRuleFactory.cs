using NScan.Lib;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.ProjectScopedRules
{
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
        new ProjectSourceCodeFilesRelatedRule(
          HasCorrectNamespacesRuleMetadata.Format(ruleDto), 
          new CorrectNamespacesInFileCheck()));
    }

    public IProjectScopedRule CreateProjectScopedRuleFrom(HasAttributesOnRuleComplementDto ruleDto)
    {
      return new ProjectScopedRuleApplicableToMatchingProject(
        ruleDto.ProjectAssemblyNamePattern,
        new ProjectSourceCodeFilesRelatedRule(
          HasAttributesOnRuleMetadata.Format(ruleDto), 
          new MethodsOfMatchingClassesAreDecoratedWithAttributeCheck(ruleDto)));
    }

    public IProjectScopedRule CreateProjectScopedRuleFrom(HasTargetFrameworkRuleComplementDto ruleDto)
    {
      Pattern expectedPropertyValue = Pattern.WithoutExclusion(ruleDto.TargetFramework);
      return 
        new ProjectScopedRuleApplicableToMatchingProject(
          ruleDto.ProjectAssemblyNamePattern,
          new HasPropertyValueRule(
            "TargetFramework", 
            expectedPropertyValue, 
            _ruleViolationFactory, 
            HasTargetFrameworkRuleMetadata.Format(ruleDto)));
    }

    public IProjectScopedRule CreateProjectScopedRuleFrom(HasPropertyRuleComplementDto ruleDto)
    {
      return 
        new ProjectScopedRuleApplicableToMatchingProject(
          ruleDto.ProjectAssemblyNamePattern,
          new HasPropertyValueRule(
            ruleDto.PropertyName, 
            ruleDto.PropertyValue, 
            _ruleViolationFactory, 
            HasPropertyRuleMetadata.Format(ruleDto)));
    }
  }
}
