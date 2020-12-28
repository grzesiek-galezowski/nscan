using System.Collections.Generic;
using System.Linq;

namespace NScan.NamespaceBasedRules
{
  //bug should this be a record?
  public record PotentialCycle(List<NamespaceName> Elements)
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

    public PotentialCycle Plus(NamespaceName namespaceName) 
      => new(Elements.Append(namespaceName).ToList());

    public List<NamespaceName> AsList()
    {
      return Elements.ToList();
    }

    public static PotentialCycle Empty() => new(new List<NamespaceName>());

    public bool IsEquivalentTo(PotentialCycle cycle)
    {
      return cycle
        .ElementsOrderedForEquivalencyComparison()
        .SequenceEqual(
          ElementsOrderedForEquivalencyComparison());
    }

    private IOrderedEnumerable<NamespaceName> ElementsOrderedForEquivalencyComparison()
    {
      return Elements
        .Distinct()
        .OrderBy(s => s.Value);
    }

  }
}
