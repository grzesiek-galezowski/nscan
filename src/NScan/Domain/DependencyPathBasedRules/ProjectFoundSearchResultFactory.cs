namespace TddXt.NScan.Domain.DependencyPathBasedRules
{
  public interface IProjectFoundSearchResultFactory
  {
    IProjectSearchResult ItemFound(IDependencyPathBasedRuleTarget foundProject, int position);
    IProjectSearchResult ItemNotFound();
  }

  public class ProjectFoundSearchResultFactory : IProjectFoundSearchResultFactory
  {
    public IProjectSearchResult ItemFound(IDependencyPathBasedRuleTarget foundProject, int position)
    {
      return new ProjectFoundSearchResult(foundProject, position);
    }

    public IProjectSearchResult ItemNotFound()
    {
      return new ProjectNotFoundSearchResult();
    }
  }
}