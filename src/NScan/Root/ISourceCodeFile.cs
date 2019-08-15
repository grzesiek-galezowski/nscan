using NScan.Domain.Domain.NamespaceBasedRules;
using NScan.Domain.Domain.ProjectScopedRules;

namespace NScan.Domain.Domain.Root
{
  public interface ISourceCodeFile : ISourceCodeFileUsingNamespaces, ISourceCodeFileInNamespace
  {}
}