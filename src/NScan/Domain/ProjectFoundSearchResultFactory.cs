namespace TddXt.NScan.Domain
{
  public interface IProjectFoundSearchResultFactory
  {
    IProjectSearchResult ItemFound(IReferencedProject foundProject, int position);
    IProjectSearchResult ItemNotFound();
  }

  public class ProjectFoundSearchResultFactory : IProjectFoundSearchResultFactory
  {
    public IProjectSearchResult ItemFound(IReferencedProject foundProject, int position)
    {
      return new ProjectFoundSearchResult(foundProject, position);
    }

    public IProjectSearchResult ItemNotFound()
    {
      return new ProjectNotFoundSearchResult();
    }
  }
}