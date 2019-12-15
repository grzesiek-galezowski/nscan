using NScan.NamespaceBasedRules;
using NScan.ProjectScopedRules;

namespace NScan.Domain
{
  public interface ISourceCodeFile : ISourceCodeFileUsingNamespaces, ISourceCodeFileInNamespace
  {}
}