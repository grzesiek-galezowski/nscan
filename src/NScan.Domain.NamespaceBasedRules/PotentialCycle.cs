using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace NScan.NamespaceBasedRules
{
  //bug should this be a record?
  public record PotentialCycle(ImmutableList<NamespaceName> Elements)
  {
    private ImmutableList<NamespaceName> Elements { get; } = Elements;

    public static PotentialCycle Empty() => new(ImmutableList<NamespaceName>.Empty);

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
      => new(Elements.Add(namespaceName).ToImmutableList());

    public List<NamespaceName> AsList()
    {
      return Elements.ToList();
    }

    public bool IsEquivalentTo(PotentialCycle other)
    {
      return other
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
