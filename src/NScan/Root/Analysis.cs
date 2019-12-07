using System.Collections.Generic;
using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.NamespaceBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Domain.Root
{
  public class Analysis
  {
    public const int ReturnCodeOk = 0;
    public const int ReturnCodeAnalysisFailed = -1;
    private readonly IAnalysisReportInProgress _analysisReportInProgress;
    private readonly INamespacesBasedRuleSet _namespacesBasedRuleSet;
    private readonly IPathRuleSet _pathRules;
    private readonly IProjectScopedRuleSet _projectScopedRules;

    private readonly ISolution _solution;
    private readonly INamespaceBasedRuleDtoVisitor _createNamespaceBasedRuleVisitor;
    private readonly IPathBasedRuleDtoVisitor _createDependencyBasedRuleVisitor;
    private readonly IProjectScopedRuleDtoVisitor _createProjectScopedRuleVisitor;

    public Analysis(
      ISolution solution,
      IPathRuleSet pathRules,
      IProjectScopedRuleSet projectScopedRules,
      INamespacesBasedRuleSet namespacesBasedRuleSet,
      IAnalysisReportInProgress analysisReportInProgress,
      IRuleFactory ruleFactory)
    {
      _solution = solution;
      _pathRules = pathRules;
      _projectScopedRules = projectScopedRules;
      _namespacesBasedRuleSet = namespacesBasedRuleSet;
      _analysisReportInProgress = analysisReportInProgress;
      //bug extract the three factories as parameters:
      var namespaceBasedRuleFactory = (INamespaceBasedRuleFactory)ruleFactory;
      var projectScopedRuleFactory = (IProjectScopedRuleFactory)ruleFactory;
      var dependencyBasedRuleFactory = (IDependencyBasedRuleFactory)ruleFactory;
      _createNamespaceBasedRuleVisitor = new CreateNamespaceBasedRuleVisitor(namespaceBasedRuleFactory, namespacesBasedRuleSet);
      _createDependencyBasedRuleVisitor = new CreateDependencyBasedRuleVisitor(dependencyBasedRuleFactory, pathRules);
      _createProjectScopedRuleVisitor = new CreateProjectScopedRuleVisitor(projectScopedRuleFactory, projectScopedRules);
    }

    public string Report => _analysisReportInProgress.AsString();
    public int ReturnCode => _analysisReportInProgress.HasViolations() ? -1 : 0;


    public static Analysis PrepareFor(IEnumerable<CsharpProjectDto> csharpProjectDtos, INScanSupport support)
    {
      var projects = 
        new CsharpWorkspaceModel(support, new RuleViolationFactory(new PlainReportFragmentsFormat()))
          .CreateProjectsDictionaryFrom(csharpProjectDtos);

      return new Analysis(new DotNetStandardSolution(projects,
          new PathCache(
            new DependencyPathFactory())),
        new PathRuleSet(), 
        new ProjectScopedRuleSet(), 
        new NamespacesBasedRuleSet(),
        new AnalysisReportInProgress(),
        new RuleFactory());
    }

    public void Run()
    {
      _solution.ResolveAllProjectsReferences();
      _solution.BuildCache();
      //_solution.PrintDebugInfo();
      _solution.Check(_pathRules, _analysisReportInProgress);
      _solution.Check(_projectScopedRules, _analysisReportInProgress);
      _solution.Check(_namespacesBasedRuleSet, _analysisReportInProgress);
    }

    public void AddDependencyPathRules(IEnumerable<DependencyPathBasedRuleUnionDto> rules)
    {
      foreach (var ruleUnionDto in rules)
      {
        ruleUnionDto.Accept(_createDependencyBasedRuleVisitor);
      }
    }
    public void AddProjectScopedRules(IEnumerable<ProjectScopedRuleUnionDto> rules)
    {
      foreach (var ruleUnionDto in rules)
      {
        ruleUnionDto.Accept(_createProjectScopedRuleVisitor);
      }
    }
    public void AddNamespaceBasedRules(IEnumerable<NamespaceBasedRuleUnionDto> rules)
    {
      foreach (var ruleUnionDto in rules)
      {
        ruleUnionDto.Accept(_createNamespaceBasedRuleVisitor);
      }
    }
  }
}