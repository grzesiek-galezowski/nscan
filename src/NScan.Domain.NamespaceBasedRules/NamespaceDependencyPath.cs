using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Design;
using System.Linq;
using Value;

namespace NScan.NamespaceBasedRules
{
  public interface INamespaceDependencyPathFormat
  {
    string ElementTerminator();
    string ElementIndentation(int elementIndex);
  }

  //bug should this be a record?
  public class NamespaceDependencyPath : ValueType<NamespaceDependencyPath>
  {
    public NamespaceDependencyPath(ImmutableList<NamespaceName> elements)
    {
      Elements = elements;
    }

    private ImmutableList<NamespaceName> Elements { get; }

    public static NamespaceDependencyPath Empty() => new(ImmutableList<NamespaceName>.Empty);
    public static NamespaceDependencyPath With(params NamespaceName[] args) => new(args.ToImmutableList());

    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
      return new ListByValue<NamespaceName>(Elements);
    }

    public override string ToString()
    {
      return ToStringFormatted(new SimpleDependencyPathFormat());
    }

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

    public List<NamespaceName> AsList() //bug remove in the end
    {
      return Elements.ToList();
    }

    public bool HasElements()
    {
      return Elements.Count > 0;
    }

    private bool IsEquivalentTo(NamespaceDependencyPath other)
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

    public string ToStringFormatted(INamespaceDependencyPathFormat format)
    {
      string result = string.Empty;
      for (var cycleElementIndex = 0; cycleElementIndex < Elements.Count; cycleElementIndex++)
      {
        var segment = Elements[cycleElementIndex].Value;
        result += format.ElementIndentation(cycleElementIndex) + segment + format.ElementTerminator();
      }

      return result;
    }

    private class SimpleDependencyPathFormat : INamespaceDependencyPathFormat
    {
      public string ElementTerminator()
      {
        return ", ";
      }

      public string ElementIndentation(int elementIndex)
      {
        return string.Empty;
      }
    }

  }
}
