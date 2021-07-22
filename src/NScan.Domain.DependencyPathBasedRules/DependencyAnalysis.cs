using System.Collections.Generic;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;

namespace NScan.DependencyPathBasedRules
{
  public interface IDependencyAnalysis
  {
    void Perform(IAnalysisReportInProgress analysisReportInProgress);
    void Add(IEnumerable<DependencyPathBasedRuleUnionDto> rules);
  }

  public class DependencyAnalysis : IDependencyAnalysis
  {
    private readonly ISolutionForDependencyPathBasedRules _solution;
    private readonly IPathRuleSet _pathRuleSet;
    private readonly IDependencyBasedRuleFactory _dependencyBasedRuleFactory;

    public DependencyAnalysis(ISolutionForDependencyPathBasedRules solution,
      IPathRuleSet pathRuleSet, IDependencyBasedRuleFactory dependencyBasedRuleFactory)
    {
      _solution = solution;
      _pathRuleSet = pathRuleSet;
      _dependencyBasedRuleFactory = dependencyBasedRuleFactory;
    }

    public void Perform(IAnalysisReportInProgress analysisReportInProgress)
    {
      _solution.ResolveAllProjectsReferences();
      _solution.BuildDependencyPathCache();
      _solution.Check(_pathRuleSet, analysisReportInProgress);
    }

    public void Add(IEnumerable<DependencyPathBasedRuleUnionDto> rules)
    {
      foreach (var ruleUnionDto in rules)
      {
        ruleUnionDto.Accept(new CreateDependencyBasedRuleVisitor(_dependencyBasedRuleFactory, _pathRuleSet));
      }
    }

    public static DependencyAnalysis PrepareFor(IEnumerable<CsharpProjectDto> csharpProjectDtos, INScanSupport support)
    {
      return new DependencyAnalysis(
        new SolutionForDependencyPathRules(
          new PathCache(
            new DependencyPathFactory()),
          new DependencyPathBasedRuleTargetFactory(support)
            .CreateDependencyPathRuleTargetsByIds(csharpProjectDtos)),
        new PathRuleSet(),
        new DependencyPathRuleFactory(
          new DependencyPathRuleViolationFactory(
            new DependencyPathReportFragmentsFormat())));
    }
  }
}
