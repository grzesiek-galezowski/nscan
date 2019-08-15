namespace NScan.Domain.Domain.DependencyPathBasedRules
{
  public interface IDependencyPathInProgress
  {
    IDependencyPathInProgress CloneWith(IDependencyPathBasedRuleTarget project);
    void FinalizeWith(IDependencyPathBasedRuleTarget finalProject);
  }
}