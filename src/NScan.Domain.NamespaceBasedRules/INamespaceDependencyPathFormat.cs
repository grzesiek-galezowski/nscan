namespace NScan.NamespaceBasedRules;

public interface INamespaceDependencyPathFormat
{
  string ElementTerminator();
  string ElementIndentation(int elementIndex);
}
