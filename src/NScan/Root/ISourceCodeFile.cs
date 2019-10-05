using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;

namespace NScan.Domain.Root
{
  public interface ISourceCodeFile : ISourceCodeFileUsingNamespaces, ISourceCodeFileInNamespace
  {}
}