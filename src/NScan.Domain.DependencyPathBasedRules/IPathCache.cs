namespace NScan.DependencyPathBasedRules;

public interface IPathCache
{
  void BuildStartingFrom(Seq<IDependencyPathBasedRuleTarget> rootProjects);
  void Check(IDependencyRule rule, IAnalysisReportInProgress report);
}
