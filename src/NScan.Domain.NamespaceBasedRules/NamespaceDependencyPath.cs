using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Value;

namespace NScan.NamespaceBasedRules
{
  public interface INamespaceDependencyPathFormat
  {
    string ElementTerminator();
    string ElementIndentation(int elementIndex);
  }

  public class NamespaceDependencyPath : ValueType<NamespaceDependencyPath>
  {
    private readonly ImmutableList<NamespaceName> _elements;

    public NamespaceDependencyPath(ImmutableList<NamespaceName> elements)
    {
      _elements = elements;
    }

    public static NamespaceDependencyPath Empty() => new(ImmutableList<NamespaceName>.Empty);
    public static NamespaceDependencyPath With(params NamespaceName[] args) => new(args.ToImmutableList());

    protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
    {
      return new ListByValue<NamespaceName>(_elements);
    }

    public override string ToString()
    {
      return ToStringFormatted(new SimpleDependencyPathFormat());
    }

    public bool IsPathToItself()
    {
      return _elements.Count == 2 && BeginsAndEndsWithTheSameElement();
    }

    public bool FormsACycleFromFirstElement()
    {
      return _elements.Count > 2 && BeginsAndEndsWithTheSameElement();
    }

    public bool ContainsACycleButNotFromFirstElement()
    {
      return ContainsANamespaceTwice() && !BeginsAndEndsWithTheSameElement();
    }

    public NamespaceDependencyPath Plus(NamespaceName namespaceName) 
      => new(_elements.Add(namespaceName).ToImmutableList());

    public List<NamespaceName> AsList() //bug remove in the end
    {
      return _elements.ToList();
    }

    public bool HasElements()
    {
      return _elements.Count > 0;
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
      return _elements
        .Distinct()
        .OrderBy(s => s.Value);
    }

    private bool BeginsAndEndsWithTheSameElement()
    {
      return _elements[0] == _elements[^1];
    }

    private bool ContainsANamespaceTwice()
    {
      return _elements.GroupBy(_ => _).Where(_ => _.Count() > 1).Sum(_ => _.Count()) > 0;
    }

    public bool IsEquivalentToAnyOf(IEnumerable<NamespaceDependencyPath> cycles)
    {
      return !cycles.Any(c => c.IsEquivalentTo(this));
    }

    public string ToStringFormatted(INamespaceDependencyPathFormat format)
    {
      string result = string.Empty;
      for (var cycleElementIndex = 0; cycleElementIndex < _elements.Count; cycleElementIndex++)
      {
        var segment = _elements[cycleElementIndex].Value;
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
