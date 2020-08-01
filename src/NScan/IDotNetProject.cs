using NScan.DependencyPathBasedRules;

namespace NScan.Domain
{
  public interface IDotNetProject : 
    IReferencedProject,
    IReferencingProject, 
    IDependencyPathBasedRuleTarget
  {}
}