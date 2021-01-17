using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;

namespace NScan.Adapter.ReadingRulesSpecification
{
  public interface IRuleDtoVisitor :
    IPathBasedRuleDtoVisitor,
    INamespaceBasedRuleDtoVisitor,
    IProjectScopedRuleDtoVisitor
  {
  }
}
