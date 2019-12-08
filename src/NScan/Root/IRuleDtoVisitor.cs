namespace NScan.Domain.Root
{
  public interface IRuleDtoVisitor :
    IPathBasedRuleDtoVisitor,
    INamespaceBasedRuleDtoVisitor,
    IProjectScopedRuleDtoVisitor
  {
  }
}