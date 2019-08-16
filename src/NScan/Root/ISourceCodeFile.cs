using NScan.Domain.NamespaceBasedRules;
using NScan.Domain.ProjectScopedRules;

namespace NScan.Domain.Root
{
  public interface ISourceCodeFile : ISourceCodeFileUsingNamespaces, ISourceCodeFileInNamespace
  {}
}