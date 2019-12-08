using NScan.DependencyPathBasedRules;
using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;

namespace NScan.Domain.Root
{
  public interface ISolution : ISolutionForDependencyPathBasedRules, ISolutionForProjectScopedRules, ISolutionForNamespaceBasedRules
  {
    void ResolveAllProjectsReferences();
    void PrintDebugInfo();
    void BuildCache();
  }
}