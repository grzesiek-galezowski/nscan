using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;

namespace TddXt.NScan.Specification.ReadingRules.Adapters
{
  public interface IRuleDtoVisitor :
    IPathBasedRuleDtoVisitor,
    INamespaceBasedRuleDtoVisitor,
    IProjectScopedRuleDtoVisitor
  {
  }
}