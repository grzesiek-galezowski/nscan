namespace MyTool
{
  public interface IPathRuleSet
  {
    void Add(IDependencyRule rule);
    void Check(IPathCache cache, IAnalysisReportInProgress report);
  }
}