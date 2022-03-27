using System.Collections.Generic;
using NScan.SharedKernel;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.NamespaceBasedRules;

public interface IProjectNamespacesAnalysis
{
  void PerformOn(IAnalysisReportInProgress analysisReportInProgress);
  void Add(IEnumerable<NamespaceBasedRuleUnionDto> rules);
}

public class ProjectNamespacesAnalysis : IProjectNamespacesAnalysis
{
  private readonly ISolutionForNamespaceBasedRules _solution;
  private readonly INamespacesBasedRuleSet _namespacesBasedRuleSet;
  private readonly INamespaceBasedRuleFactory _namespaceBasedRuleFactory;

  public ProjectNamespacesAnalysis(ISolutionForNamespaceBasedRules solution,
    INamespacesBasedRuleSet namespacesBasedRuleSet,
    INamespaceBasedRuleFactory namespaceBasedRuleFactory)
  {
    _solution = solution;
    _namespacesBasedRuleSet = namespacesBasedRuleSet;
    _namespaceBasedRuleFactory = namespaceBasedRuleFactory;
  }

  public void PerformOn(IAnalysisReportInProgress analysisReportInProgress)
  {
    _solution.BuildNamespacesCache();
    _solution.Check(_namespacesBasedRuleSet, analysisReportInProgress);
  }

  public void Add(IEnumerable<NamespaceBasedRuleUnionDto> rules)
  {
    foreach (var ruleUnionDto in rules)
    {
      ruleUnionDto.Accept(new CreateNamespaceBasedRuleVisitor(_namespaceBasedRuleFactory, _namespacesBasedRuleSet));
    }
  }

  public static ProjectNamespacesAnalysis PrepareFor(IEnumerable<CsharpProjectDto> csharpProjectDtos)
  {
    return new ProjectNamespacesAnalysis(
      new SolutionForNamespaceBasedRules(new NamespaceBasedRuleTargetFactory()
        .NamespaceBasedRuleTargets(csharpProjectDtos)),
      new NamespacesBasedRuleSet(), 
      new NamespaceBasedRuleFactory(
        new NamespaceBasedRuleViolationFactory(
          new NamespaceBasedReportFragmentsFormat())));
  }
}