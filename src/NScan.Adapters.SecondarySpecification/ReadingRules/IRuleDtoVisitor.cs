using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;

namespace NScan.Adapters.SecondarySpecification.ReadingRules;

public interface IRuleDtoVisitor :
  IPathBasedRuleDtoVisitor,
  INamespaceBasedRuleDtoVisitor,
  IProjectScopedRuleDtoVisitor
{
}