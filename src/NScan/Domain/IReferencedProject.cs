using GlobExpressions;
using TddXt.NScan.ReadingRules.Ports;

namespace TddXt.NScan.Domain
{
  public interface IReferencedProject : IDependencyPathBasedRuleTarget
  {
    void Print(int nestingLevel);
    void AddReferencingProject(ProjectId projectId, IReferencingProject referencingProject);
    void ResolveAsReferenceOf(IReferencingProject project);
    bool HasProjectAssemblyNameMatching(Pattern glob);
    bool HasProjectAssemblyNameMatching(Glob glob);
    bool HasPackageReferenceMatching(Glob packagePattern);
    bool HasAssemblyReferenceWithNameMatching(Glob pattern);

    string ToString();
  }
}