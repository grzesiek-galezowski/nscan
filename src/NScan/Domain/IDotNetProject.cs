using TddXt.NScan.App;

namespace TddXt.NScan.Domain
{
  //bug further separate interfaces - scoped rules need different and path rules need different

  public interface IDotNetProject : IReferencedProject, IReferencingProject, IProjectScopedRuleTarget
  {

  }
}