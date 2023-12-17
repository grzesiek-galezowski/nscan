using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Value;

namespace NScan.NamespaceBasedRules;

public interface INamespaceDependencyPathFormat
{
  string ElementTerminator();
  string ElementIndentation(int elementIndex);
}

public class NamespaceDependencyPath(ImmutableList<NamespaceName> elements) : ValueType<NamespaceDependencyPath>
{
  private static readonly SimpleDependencyPathFormat DefaultFormat = new();

  public static NamespaceDependencyPath Empty() => new(ImmutableList<NamespaceName>.Empty);
  public static NamespaceDependencyPath With(params NamespaceName[] args) => new(args.ToImmutableList());

  protected override IEnumerable<object> GetAllAttributesToBeUsedForEquality()
  {
    yield return new ListByValue<NamespaceName>(elements);
  }

  public override string ToString()
  {
    return ToStringFormatted(DefaultFormat);
  }

  public bool IsPathToItself()
  {
    return elements.Count == 2 && BeginsAndEndsWithTheSameElement();
  }

  public bool FormsACycleFromFirstElement()
  {
    return elements.Count > 2 && BeginsAndEndsWithTheSameElement();
  }

  public bool ContainsACycleButNotFromFirstElement()
  {
    return ContainsANamespaceTwice() && !BeginsAndEndsWithTheSameElement();
  }

  public NamespaceDependencyPath Plus(NamespaceName namespaceName) 
    => new(elements.Add(namespaceName).ToImmutableList());

  public bool HasElements()
  {
    return elements.Count > 0;
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
    return elements
      .Distinct()
      .OrderBy(s => s.ToString());
  }

  private bool BeginsAndEndsWithTheSameElement()
  {
    return elements[0] == elements[^1];
  }

  private bool ContainsANamespaceTwice()
  {
    return elements.GroupBy(_ => _).Where(_ => _.Count() > 1).Sum(_ => _.Count()) > 0;
  }

  public bool IsEquivalentToAnyOf(IEnumerable<NamespaceDependencyPath> cycles)
  {
    return !cycles.Any(c => c.IsEquivalentTo(this));
  }

  public string ToStringFormatted(INamespaceDependencyPathFormat format)
  {
    string result = string.Empty;
    for (var cycleElementIndex = 0; cycleElementIndex < elements.Count; cycleElementIndex++)
    {
      var segment = elements[cycleElementIndex].ToString();
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
