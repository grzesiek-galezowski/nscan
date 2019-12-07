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
  public class Analysis //bug extract two more analysis classes
  {
    public const int ReturnCodeOk = 0;
    public const int ReturnCodeAnalysisFailed = -1;
    private readonly IAnalysisReportInProgress _analysisReportInProgress;
    private readonly ISolution _solution;
    private readonly ISpecificKindOfRuleAnalysis<DependencyPathBasedRuleUnionDto> _dependencyAnalysis;
    private readonly ISpecificKindOfRuleAnalysis<ProjectScopedRuleUnionDto> _projectAnalysis;
    private readonly ISpecificKindOfRuleAnalysis<NamespaceBasedRuleUnionDto>  _projectNamespacesAnalysis;

    public Analysis(
      ISolution solution,
      IPathRuleSet pathRules,
      IProjectScopedRuleSet projectScopedRules,
      INamespacesBasedRuleSet namespacesBasedRuleSet,
      IAnalysisReportInProgress analysisReportInProgress,
      IRuleFactory ruleFactory)
    {
      _solution = solution;
      _analysisReportInProgress = analysisReportInProgress;
      var namespaceBasedRuleFactory = (INamespaceBasedRuleFactory)ruleFactory;
      var projectScopedRuleFactory = (IProjectScopedRuleFactory)ruleFactory;
      var dependencyBasedRuleFactory = (IDependencyBasedRuleFactory)ruleFactory;
      _dependencyAnalysis = new DependencyAnalysis(pathRules, dependencyBasedRuleFactory);
      _projectAnalysis = new ProjectAnalysis(projectScopedRules, projectScopedRuleFactory);
      _projectNamespacesAnalysis = new ProjectNamespacesAnalysis(namespacesBasedRuleSet, namespaceBasedRuleFactory);
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
      _dependencyAnalysis.PerformOn(_solution, _analysisReportInProgress);
      _projectAnalysis.PerformOn(_solution, _analysisReportInProgress);
      _projectNamespacesAnalysis.PerformOn(_solution, _analysisReportInProgress);
    }

    public void AddDependencyPathRules(IEnumerable<DependencyPathBasedRuleUnionDto> rules)
    {
      _dependencyAnalysis.Add(rules);
    }

    public void AddProjectScopedRules(IEnumerable<ProjectScopedRuleUnionDto> rules)
    {
      _projectAnalysis.Add(rules);
    }

    public void AddNamespaceBasedRules(IEnumerable<NamespaceBasedRuleUnionDto> rules)
    {
      _projectNamespacesAnalysis.Add(rules);
    }
  }
}