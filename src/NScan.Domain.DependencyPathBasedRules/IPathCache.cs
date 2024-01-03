using System.Collections.Generic;

namespace NScan.DependencyPathBasedRules;

public interface IPathCache
{
  void BuildStartingFrom(IEnumerable<IDependencyPathBasedRuleTarget> rootProjects);
  void Check(IDependencyRule rule, IAnalysisReportInProgress report);
}