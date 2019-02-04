using TddXt.NScan.Domain.NamespaceBasedRules;
using TddXt.NScan.Domain.ProjectScopedRules;

namespace TddXt.NScan.Domain.Root
{
  public interface ISourceCodeFile : ISourceCodeFileUsingNamespaces, ISourceCodeFileInNamespace
  {
  }
}