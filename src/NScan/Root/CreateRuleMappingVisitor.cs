using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Domain.Root
{
  //bug split the visitor
  public class CreateProjectScopedRuleVisitor : IProjectScopedRuleDtoVisitor
  {
    private readonly IProjectScopedRuleFactory _projectScopedRuleFactory;
    private readonly IProjectScopedRuleSet _projectScopedRules;

    public CreateProjectScopedRuleVisitor(IProjectScopedRuleFactory projectScopedRuleFactory,
      IProjectScopedRuleSet projectScopedRules)
    {
      _projectScopedRuleFactory = projectScopedRuleFactory;
      _projectScopedRules = projectScopedRules;
    }

    public void Visit(HasTargetFrameworkRuleComplementDto arg)
    {
      var rule = _projectScopedRuleFactory.CreateProjectScopedRuleFrom(arg);
      _projectScopedRules.Add(rule);
    }

    public void Visit(HasAttributesOnRuleComplementDto dto)
    {
      var rule = _projectScopedRuleFactory.CreateProjectScopedRuleFrom(dto);
      _projectScopedRules.Add(rule);
    }

    public void Visit(CorrectNamespacesRuleComplementDto dto)
    {
      var rule = _projectScopedRuleFactory.CreateProjectScopedRuleFrom(dto);
      _projectScopedRules.Add(rule);
    }
  }

  public class CreateDependencyBasedRuleVisitor : IPathBasedRuleDtoVisitor
  {
    private readonly IDependencyBasedRuleFactory _dependencyBasedRuleFactory;
    private readonly IPathRuleSet _pathRules;

    public CreateDependencyBasedRuleVisitor(IDependencyBasedRuleFactory dependencyBasedRuleFactory,
      IPathRuleSet pathRules)
    {
      _dependencyBasedRuleFactory = dependencyBasedRuleFactory;
      _pathRules = pathRules;
    }

    public void Visit(IndependentRuleComplementDto dto)
    {
      var rule = _dependencyBasedRuleFactory.CreateDependencyRuleFrom(dto);
      _pathRules.Add(rule);
    }
  }

  public class CreateNamespaceBasedRuleVisitor : INamespaceBasedRuleDtoVisitor
  {
    private readonly INamespaceBasedRuleFactory _namespaceBasedRuleFactory;
    private readonly INamespacesBasedRuleSet _namespacesBasedRuleSet;

    public CreateNamespaceBasedRuleVisitor(
      INamespaceBasedRuleFactory namespaceBasedRuleFactory,
      INamespacesBasedRuleSet namespacesBasedRuleSet)
    {
      _namespaceBasedRuleFactory = namespaceBasedRuleFactory;
      _namespacesBasedRuleSet = namespacesBasedRuleSet;
    }

    public void Visit(NoCircularUsingsRuleComplementDto dto)
    {
      var rule = _namespaceBasedRuleFactory.CreateNamespacesBasedRuleFrom(dto);
      _namespacesBasedRuleSet.Add(rule);
    }

    public void Visit(NoUsingsRuleComplementDto dto)
    {
      var rule = _namespaceBasedRuleFactory.CreateNamespacesBasedRuleFrom(dto);
      _namespacesBasedRuleSet.Add(rule);
    }
  }

  public class CreateRuleMappingVisitor : IRuleDtoVisitor
  {
    private readonly IProjectScopedRuleDtoVisitor _createProjectScopedRuleVisitor;
    private readonly IPathBasedRuleDtoVisitor _createDependencyBasedRuleVisitor;
    private readonly CreateNamespaceBasedRuleVisitor _createNamespaceBasedRuleVisitor;

    public CreateRuleMappingVisitor(
      INamespacesBasedRuleSet namespacesBasedRuleSet, 
      IProjectScopedRuleSet projectScopedRules, 
      IPathRuleSet pathRules, 
      INamespaceBasedRuleFactory namespaceBasedRuleFactory, 
      IDependencyBasedRuleFactory dependencyBasedRuleFactory, 
      IProjectScopedRuleFactory projectScopedRuleFactory)
    {
      _createNamespaceBasedRuleVisitor = new CreateNamespaceBasedRuleVisitor(namespaceBasedRuleFactory, namespacesBasedRuleSet);
      _createDependencyBasedRuleVisitor = new CreateDependencyBasedRuleVisitor(dependencyBasedRuleFactory, pathRules);
      _createProjectScopedRuleVisitor = new CreateProjectScopedRuleVisitor(projectScopedRuleFactory, projectScopedRules);
    }

    public void Visit(HasTargetFrameworkRuleComplementDto arg)
    {
      _createProjectScopedRuleVisitor.Visit(arg);
    }

    public void Visit(HasAttributesOnRuleComplementDto dto)
    {
      _createProjectScopedRuleVisitor.Visit(dto);
    }

    public void Visit(NoCircularUsingsRuleComplementDto dto)
    {
      _createNamespaceBasedRuleVisitor.Visit(dto);
    }

    public void Visit(NoUsingsRuleComplementDto dto)
    {
      _createNamespaceBasedRuleVisitor.Visit(dto);
    }

    public void Visit(CorrectNamespacesRuleComplementDto dto)
    {
      _createProjectScopedRuleVisitor.Visit(dto);
    }

    public void Visit(IndependentRuleComplementDto dto)
    {
      _createDependencyBasedRuleVisitor.Visit(dto);
    }
  }
}