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

public class ProjectNamespacesAnalysis(
  ISolutionForNamespaceBasedRules solution,
  INamespacesBasedRuleSet namespacesBasedRuleSet,
  INamespaceBasedRuleFactory namespaceBasedRuleFactory)
  : IProjectNamespacesAnalysis
{
  public void PerformOn(IAnalysisReportInProgress analysisReportInProgress)
  {
    solution.BuildNamespacesCache();
    solution.Check(namespacesBasedRuleSet, analysisReportInProgress);
  }

  public void Add(IEnumerable<NamespaceBasedRuleUnionDto> rules)
  {
    foreach (var ruleUnionDto in rules)
    {
      ruleUnionDto.Accept(new CreateNamespaceBasedRuleVisitor(namespaceBasedRuleFactory, namespacesBasedRuleSet));
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
