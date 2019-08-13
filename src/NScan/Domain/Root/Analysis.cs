using System.Collections.Generic;
using System.Linq;
using NScan.Lib;
using TddXt.NScan.Domain.DependencyPathBasedRules;
using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.ProjectScopedRules;
using TddXt.NScan.Domain.SharedKernel;
using TddXt.NScan.NotifyingSupport.Ports;
using TddXt.NScan.ReadingRules.Ports;
using TddXt.NScan.ReadingSolution.Lib;
using TddXt.NScan.ReadingSolution.Ports;

namespace TddXt.NScan.Domain.Root
{

  public interface IRuleDtoVisitor : IUnion5Visitor<
    IndependentRuleComplementDto,
    CorrectNamespacesRuleComplementDto,
    NoCircularUsingsRuleComplementDto,
    HasAttributesOnRuleComplementDto,
    HasTargetFrameworkRuleComplementDto
  >
  {
  }

  public class CreateRuleMappingVisitor : IRuleDtoVisitor
  {
    private readonly IRuleFactory _ruleFactory;
    private readonly INamespacesBasedRuleSet _namespacesBasedRuleSet;
    private readonly IProjectScopedRuleSet _projectScopedRules;
    private readonly IPathRuleSet _pathRules;

    public CreateRuleMappingVisitor(
      IRuleFactory ruleFactory, 
      INamespacesBasedRuleSet namespacesBasedRuleSet, 
      IProjectScopedRuleSet projectScopedRules, 
      IPathRuleSet pathRules)
    {
      _ruleFactory = ruleFactory;
      _namespacesBasedRuleSet = namespacesBasedRuleSet;
      _projectScopedRules = projectScopedRules;
      _pathRules = pathRules;
    }

    public void Visit(HasTargetFrameworkRuleComplementDto arg)
    {
      var rule = _ruleFactory.CreateProjectScopedRuleFrom(arg);
      _projectScopedRules.Add(rule);
    }

    public void Visit(HasAttributesOnRuleComplementDto dto)
    {
      var rule = _ruleFactory.CreateProjectScopedRuleFrom(dto);
      _projectScopedRules.Add(rule);
    }

    public void Visit(NoCircularUsingsRuleComplementDto dto)
    {
      var rule = _ruleFactory.CreateNamespacesBasedRuleFrom(dto);
      _namespacesBasedRuleSet.Add(rule);
    }

    public void Visit(CorrectNamespacesRuleComplementDto dto)
    {
      var rule = _ruleFactory.CreateProjectScopedRuleFrom(dto);
      _projectScopedRules.Add(rule);
    }

    public void Visit(IndependentRuleComplementDto dto)
    {
      var rule = _ruleFactory.CreateDependencyRuleFrom(dto);
      _pathRules.Add(rule);
    }
  }

  public class Analysis
  {
    public const int ReturnCodeOk = 0;
    public const int ReturnCodeAnalysisFailed = -1;
    private readonly IAnalysisReportInProgress _analysisReportInProgress;
    private readonly INamespacesBasedRuleSet _namespacesBasedRuleSet;
    private readonly IPathRuleSet _pathRules;
    private readonly IProjectScopedRuleSet _projectScopedRules;

    private readonly ISolution _solution;
    private readonly IRuleDtoVisitor _createRuleMappingVisitor;

    public Analysis(ISolution solution,
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
      _createRuleMappingVisitor = new CreateRuleMappingVisitor(ruleFactory, namespacesBasedRuleSet, projectScopedRules, pathRules);
    }

    public string Report => _analysisReportInProgress.AsString();
    public int ReturnCode => _analysisReportInProgress.HasViolations() ? -1 : 0;


    public static Analysis PrepareFor(IReadOnlyList<XmlProject> xmlProjects, INScanSupport support)
    {
      var projects = 
        new CsharpWorkspaceModel(support, new RuleViolationFactory(new PlainReportFragmentsFormat()))
          .CreateProjectsDictionaryFrom(xmlProjects.Select(p => new XmlProjectDataAccess(p)));

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

    public void AddRules(IEnumerable<RuleUnionDto> rules)
    {
      foreach (var ruleUnionDto in rules)
      {
        ruleUnionDto.Accept(_createRuleMappingVisitor);
      }
    }
  }
}