using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;

namespace NScan.Domain.Root
{
  public interface IRuleDtoVisitor :
    IPathBasedRuleDtoVisitor,
    INamespaceBasedRuleDtoVisitor,
    IProjectScopedRuleDtoVisitor
  {
  }
}