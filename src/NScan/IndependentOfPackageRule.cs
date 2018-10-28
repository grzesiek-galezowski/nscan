namespace TddXt.NScan
{
  public class IndependentOfPackageRule : IDependencyRule
  {
    private readonly string _dependingId;
    private readonly string _dependencyId;

    public IndependentOfPackageRule(string dependingId, string dependencyId)
    {
      _dependingId = dependingId;
      _dependencyId = dependencyId;
    }

    public void Check(IAnalysisReportInProgress report, IProjectDependencyPath dependencyPath)
    {
      throw new System.NotImplementedException();
    }
  }
}