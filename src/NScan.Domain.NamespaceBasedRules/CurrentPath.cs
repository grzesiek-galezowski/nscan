using System.Collections.Generic;
using System.Linq;

namespace NScan.NamespaceBasedRules
{
  public record CurrentPath(List<NamespaceName> Elements)
  {
    private List<NamespaceName> Elements { get; } = Elements;

    public bool ConsistsSolelyOf(NamespaceName namespaceName)
    {
      return Elements.Count == 1 && Elements[0] == namespaceName;
    }

    public bool BeginsWith(NamespaceName namespaceName)
    {
      return Elements.Contains(namespaceName) && Elements[0] == namespaceName;
    }

    public bool ContainsButDoesNotBeginWith(NamespaceName namespaceName)
    {
      return Elements.Contains(namespaceName) && Elements[0] != namespaceName;
    }

    public CurrentPath Plus(NamespaceName namespaceName) 
      => new(Elements.Append(namespaceName).ToList());

    public IOrderedEnumerable<NamespaceName> ElementsOrderedForEquivalencyComparison()
    {
      return Elements
        .Distinct()
        .OrderBy(s => s.Value);
    }

    public List<NamespaceName> AsList()
    {
      return Elements.ToList();
    }

    public static CurrentPath Empty()
    {
      return new CurrentPath(new List<NamespaceName>());
    }
  }
}
