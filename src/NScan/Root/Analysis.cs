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
  public class DependencyAnalysis
  {
    private readonly IPathRuleSet _pathRuleSet;
    private readonly IDependencyBasedRuleFactory _dependencyBasedRuleFactory;

    public DependencyAnalysis(IPathRuleSet pathRuleSet, IDependencyBasedRuleFactory dependencyBasedRuleFactory)
    {
      _pathRuleSet = pathRuleSet;
      _dependencyBasedRuleFactory = dependencyBasedRuleFactory;
    }

    public void Perform(IAnalysisReportInProgress analysisReportInProgress, ISolution solution)
    {
      solution.Check(_pathRuleSet, analysisReportInProgress);
    }

    public void Add(IEnumerable<DependencyPathBasedRuleUnionDto> rules)
    {
      foreach (var ruleUnionDto in rules)
      {
        ruleUnionDto.Accept(new CreateDependencyBasedRuleVisitor(_dependencyBasedRuleFactory, _pathRuleSet));
      }
    }
  }

  public class Analysis //bug extract two more analysis classes
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
    private readonly DependencyAnalysis _dependencyAnalysis;

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
      _dependencyAnalysis = new DependencyAnalysis(_pathRules, dependencyBasedRuleFactory);
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
      _dependencyAnalysis.Perform(_analysisReportInProgress, _solution);
      CheckProject(_analysisReportInProgress, _projectScopedRules, _solution);
      CheckNamespaces(_analysisReportInProgress, _namespacesBasedRuleSet, _solution);
    }

    private static void CheckNamespaces(IAnalysisReportInProgress analysisReportInProgress, INamespacesBasedRuleSet namespacesBasedRuleSet, ISolution solution)
    {
      solution.Check(namespacesBasedRuleSet, analysisReportInProgress);
    }

    private static void CheckProject(IAnalysisReportInProgress analysisReportInProgress, IProjectScopedRuleSet projectScopedRuleSet, ISolution solution)
    {
      solution.Check(projectScopedRuleSet, analysisReportInProgress);
    }

    public void AddDependencyPathRules(IEnumerable<DependencyPathBasedRuleUnionDto> rules)
    {
      _dependencyAnalysis.Add(rules);
    }

    public void AddProjectScopedRules(IEnumerable<ProjectScopedRuleUnionDto> rules)
    {
      AddProject(_createProjectScopedRuleVisitor, rules);
    }

    private static void AddProject(IProjectScopedRuleDtoVisitor createProjectScopedRuleVisitor, IEnumerable<ProjectScopedRuleUnionDto> rules)
    {
      foreach (var ruleUnionDto in rules)
      {
        ruleUnionDto.Accept(createProjectScopedRuleVisitor);
      }
    }

    public void AddNamespaceBasedRules(IEnumerable<NamespaceBasedRuleUnionDto> rules)
    {
      AddNamespace(_createNamespaceBasedRuleVisitor, rules);
    }

    private static void AddNamespace(INamespaceBasedRuleDtoVisitor createNamespaceBasedRuleVisitor, IEnumerable<NamespaceBasedRuleUnionDto> rules)
    {
      foreach (var ruleUnionDto in rules)
      {
        ruleUnionDto.Accept(createNamespaceBasedRuleVisitor);
      }
    }
  }
}