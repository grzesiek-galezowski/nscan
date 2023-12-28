namespace NScan.DependencyPathBasedRules;

public interface IDotNetProject : 
  IReferencedProject,
  IReferencingProject, 
  IDependencyPathBasedRuleTarget;