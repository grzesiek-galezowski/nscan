using TddXt.NScan.CompositionRoot;

namespace TddXt.NScan.Domain
{
  public class IndependentRule : IDependencyRule
  {
    private readonly IDescribedDependencyCondition _condition;
    private readonly Pattern _dependingAssemblyNamePattern;

    public IndependentRule(IDescribedDependencyCondition dependencyCondition, Pattern dependingAssemblyNamePattern)
    {
      _condition = dependencyCondition;
      _dependingAssemblyNamePattern = dependingAssemblyNamePattern;
    }

    public void Check(IAnalysisReportInProgress report, IProjectDependencyPath dependencyPath)
    {
      var dependingAssembly = dependencyPath.AssemblyWithNameMatching(_dependingAssemblyNamePattern);

      if (dependingAssembly.Exists())
      {
        var dependencyAssembly = dependencyPath.AssemblyMatching(_condition, dependingAssembly);
        if (dependencyAssembly.IsNotBefore(dependingAssembly))
        {
          report.PathViolation(
            _condition.Description(),
            dependencyPath.SegmentBetween(dependingAssembly, dependencyAssembly));
        }
        else
        {
          report.FinishedChecking(_condition.Description());
        }
      }
      else
      {
        report.FinishedChecking(_condition.Description());
      }
    }
  }


}