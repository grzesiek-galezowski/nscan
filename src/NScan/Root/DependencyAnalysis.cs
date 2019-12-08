using System.Collections.Generic;
using NScan.DependencyPathBasedRules;
using NScan.SharedKernel;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;

namespace NScan.Domain.Root
{
  public interface IDependencyAnalysis
  {
    void PerformOn(ISolution solution, IAnalysisReportInProgress analysisReportInProgress);
    void Add(IEnumerable<DependencyPathBasedRuleUnionDto> rules);
  }

  public class DependencyAnalysis : IDependencyAnalysis
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