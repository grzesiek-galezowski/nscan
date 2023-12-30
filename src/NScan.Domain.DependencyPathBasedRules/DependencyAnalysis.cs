using System.Collections.Generic;
using LanguageExt;
using NScan.SharedKernel;
using NScan.SharedKernel.NotifyingSupport.Ports;
using NScan.SharedKernel.ReadingSolution.Ports;
using NScan.SharedKernel.RuleDtos.DependencyPathBased;

namespace NScan.DependencyPathBasedRules;

public interface IDependencyAnalysis
{
  void Perform(IAnalysisReportInProgress analysisReportInProgress);
  void Add(IEnumerable<DependencyPathBasedRuleUnionDto> rules);
}

public class DependencyAnalysis(
  ISolutionForDependencyPathBasedRules solution,
  IPathRuleSet pathRuleSet,
  IDependencyBasedRuleFactory dependencyBasedRuleFactory)
  : IDependencyAnalysis
{
  public void Perform(IAnalysisReportInProgress analysisReportInProgress)
  {
    solution.ResolveAllProjectsReferences();
    solution.BuildDependencyPathCache();
    solution.Check(pathRuleSet, analysisReportInProgress);
  }

  public void Add(IEnumerable<DependencyPathBasedRuleUnionDto> rules)
  {
    foreach (var ruleUnionDto in rules)
    {
      ruleUnionDto.Accept(new CreateDependencyBasedRuleVisitor(dependencyBasedRuleFactory, pathRuleSet));
    }
  }

  public static DependencyAnalysis PrepareFor(Seq<CsharpProjectDto> csharpProjectDtos, INScanSupport support)
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
