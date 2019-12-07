using System.Collections.Generic;
using NScan.NamespaceBasedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.NamespaceBased;

namespace NScan.Domain.Root
{
  //bug UT
  public class ProjectNamespacesAnalysis : ISpecificKindOfRuleAnalysis<NamespaceBasedRuleUnionDto>
  {
    private readonly INamespacesBasedRuleSet _namespacesBasedRuleSet;
    private readonly INamespaceBasedRuleFactory _namespaceBasedRuleFactory;

    public ProjectNamespacesAnalysis(
      INamespacesBasedRuleSet namespacesBasedRuleSet,
      INamespaceBasedRuleFactory namespaceBasedRuleFactory)
    {
      _namespacesBasedRuleSet = namespacesBasedRuleSet;
      _namespaceBasedRuleFactory = namespaceBasedRuleFactory;
    }

    public void PerformOn(ISolution solution, IAnalysisReportInProgress analysisReportInProgress)
    {
      solution.Check(_namespacesBasedRuleSet, analysisReportInProgress);
    }

    public void Add(IEnumerable<NamespaceBasedRuleUnionDto> rules)
    {
      foreach (var ruleUnionDto in rules)
      {
        ruleUnionDto.Accept(new CreateNamespaceBasedRuleVisitor(_namespaceBasedRuleFactory, _namespacesBasedRuleSet));
      }
    }
  }
}