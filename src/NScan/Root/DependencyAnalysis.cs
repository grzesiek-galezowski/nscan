using System.Collections.Generic;
using NScan.DependencyPathBasedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;
using NScan.SharedKernel.RuleDtos.ProjectScoped;

namespace NScan.Domain.Root
{
  //bug UT
  public class DependencyAnalysis : ISpecificKindOfRuleAnalysis<DependencyPathBasedRuleUnionDto>
  {
    private readonly IPathRuleSet _pathRuleSet;
    private readonly IDependencyBasedRuleFactory _dependencyBasedRuleFactory;

    public DependencyAnalysis(IPathRuleSet pathRuleSet, IDependencyBasedRuleFactory dependencyBasedRuleFactory)
    {
      _pathRuleSet = pathRuleSet;
      _dependencyBasedRuleFactory = dependencyBasedRuleFactory;
    }

    public void PerformOn(ISolution solution, IAnalysisReportInProgress analysisReportInProgress)
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
}