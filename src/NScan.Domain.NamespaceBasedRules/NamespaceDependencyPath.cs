using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace NScan.NamespaceBasedRules
{
  //bug should this be a record?
  public record NamespaceDependencyPath(ImmutableList<NamespaceName> Elements)
  {
    private ImmutableList<NamespaceName> Elements { get; } = Elements; //bug make private

    public static NamespaceDependencyPath Empty() => new(ImmutableList<NamespaceName>.Empty);

    public bool IsPathToItself()
    {
      return Elements.Count == 2 && BeginsAndEndsWithTheSameElement();
    }

    public bool FormsACycleFromFirstElement()
    {
      return Elements.Count > 2 && BeginsAndEndsWithTheSameElement();
    }

    public bool ContainsACycleButNotFromFirstElement()
    {
      return ContainsANamespaceTwice() && !BeginsAndEndsWithTheSameElement();
    }

    public NamespaceDependencyPath Plus(NamespaceName namespaceName) 
      => new(Elements.Add(namespaceName).ToImmutableList());

    public List<NamespaceName> AsList()
    {
      return Elements.ToList();
    }

    public bool IsEquivalentTo(NamespaceDependencyPath other)
    {
      return other
        .ElementsOrderedForEquivalencyComparison()
        .SequenceEqual(
          ElementsOrderedForEquivalencyComparison());
    }

    public bool HasElements()
    {
      return Elements.Count > 0;
    }

    private IOrderedEnumerable<NamespaceName> ElementsOrderedForEquivalencyComparison()
    {
      return Elements
        .Distinct()
        .OrderBy(s => s.Value);
    }

    private bool BeginsAndEndsWithTheSameElement()
    {
      return Elements[0] == Elements[^1];
    }

    private bool ContainsANamespaceTwice()
    {
      return Elements.GroupBy(_ => _).Where(_ => _.Count() > 1).Sum(_ => _.Count()) > 0;
    }

    public bool IsEquivalentToAnyOf(IEnumerable<NamespaceDependencyPath> cycles)
    {
      //A->B->A and B->A->B are the same cycle, no need to report twice
      return !cycles.Any(c => c.IsEquivalentTo(this));
    }
  }
}
